using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.IdentityModel.Tokens.Jwt;
using WebDevelopment.Client.Services;
using WebDevelopment.Client.StringConstants;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IConfiguration _config;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly Random _random = new();
        private readonly ILogger<ApiClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ApiClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, ProtectedLocalStorageService localStorage, AuthenticationStateProvider authStateProvider, IConfiguration config, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _config = config;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                    {
                        var baseDelay = Math.Pow(2, retryAttempt);
                        var jitter = _random.NextDouble() * 0.5 + 0.5;
                        return TimeSpan.FromSeconds(baseDelay * jitter);
                    },
                    onRetry: (response, timeSpan, retryAttempt, context) =>
                    {
                        logger.LogWarning($"Retry {retryAttempt} for {context.PolicyKey} at {context.OperationKey}, due to: {response.Result.StatusCode}. Waiting {timeSpan} before next retry.");
                    }
                );
        }

        long? GetExpirationDate(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken.Payload.TryGetValue("exp", out var expClaim))
            {
                var expSeconds = Convert.ToInt64(expClaim);
                return expSeconds;
            }

            return null; // `exp` claim not found
        }
        private async Task SetAuthorizeHeader()
        {
            try
            {
                var accessToken = await _localStorage.GetItemAsync<string>(AuthConstants.AccessToken);
                var refreshToken = await _localStorage.GetItemAsync<string>(AuthConstants.RefreshToken);

                if (!string.IsNullOrEmpty(accessToken) || !string.IsNullOrEmpty(refreshToken))
                {
                    var expirationDate = GetExpirationDate(accessToken!);

                    if (expirationDate!.Value < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    {
                        var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{_config["WebApi"]}/api/v1/Auth/refresh?refreshToken={refreshToken}"));

                        if (response.IsSuccessStatusCode)
                        {
                            var result = JsonConvert.DeserializeObject<Response<string>>(await response.Content.ReadAsStringAsync());

                            await ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(new TokenResponse(result.Data, refreshToken));
                            _httpClient.DefaultRequestHeaders.Authorization = null;
                            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {result.Data}");
                        }
                        else
                        {
                            await ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
                        }
                    }
                    else
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = null;
                        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting the authorization header.");
            }
        }

        public async Task<T> GetFromJsonAsync<T>(string path, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(path));

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API request failed: {content}");
                    return default;
                }

                try
                {
                    var content = await response.Content.ReadFromJsonAsync<T>();
                    return content!;
                }
                catch (JsonException ex)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to deserialize JSON response: {rawContent}", ex);
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a GET request to {Path}", path);
                return default;
            }
        }

        public async Task<string> GetStringAsync(string path, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(path));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API request failed: {response.Content.ToString()}");
                    return null;
                }

                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError("Failed to read response content as string. Exeption: {ex}", ex);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a GET request to {Path}", path);
                return null;
            }
        }

        public async Task<T1> PostAsync<T1, T2>(string path, T2? postModel, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var res = await _retryPolicy.ExecuteAsync(() => _httpClient.PostAsJsonAsync(path, postModel));
                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    _logger.LogError($"Error: Status Code {(int)res.StatusCode} ({res.StatusCode}) - {errorContent}");
                    return default;
                }

                return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a POST request to {Path}", path);
                return default;
            }
        }

        public async Task<T1> PutAsync<T1, T2>(string path, T2 postModel, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var res = await _retryPolicy.ExecuteAsync(() => _httpClient.PutAsJsonAsync(path, postModel));
                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    _logger.LogError($"Error: Status Code {(int)res.StatusCode} ({res.StatusCode}) - {errorContent}");
                    return default;
                }

                return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a PUT request to {Path}", path);
                return default;
            }
        }

        public async Task<T> DeleteAsync<T>(string path, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.DeleteAsync(path));

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error: Status Code {(int)response.StatusCode} ({response.StatusCode}) - {errorContent}");
                    return default;
                }

                var content = await response.Content.ReadFromJsonAsync<T>();
                return content!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a DELETE request to {Path}", path);
                return default;
            }
        }
        public async Task<string> DeleteAsync(string path, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.DeleteAsync(path));

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error: Status Code {(int)response.StatusCode} ({response.StatusCode}) - {errorContent}");
                    return default;
                }

                var content = await response.Content.ReadAsStringAsync();
                return content!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a DELETE request to {Path}", path);
                return default;
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonGetResponseAsync<T>(string path, T model, bool IsAuthorization = true)
        {
            try
            {
                if (IsAuthorization)
                    await SetAuthorizeHeader();

                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.PostAsJsonAsync(path, model));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making a POST request to {Path}", path);
                throw;
            }
        }
    }
}
