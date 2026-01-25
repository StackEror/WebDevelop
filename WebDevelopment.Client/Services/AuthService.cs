using System.Text;
using System.Text.Json;
using WebDevelopment.Client.Interfaces;
using WebDevelopment.Client.Models.Authentication;
using WebDevelopment.Client.StringConstants;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services;


public class AuthService(
    ProtectedLocalStorageService protectedLocalStorageService,
    CustomAuthenticationStateProvider customAuthenticationStateProvider,
    HttpClient httpClient
    ) : IAuthService
{
    private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Response> Register(RegisterUserModel userModel)
    {
        var jsonContent = GetJson(userModel);
        var response = await httpClient.PostAsync("api/auth/register", jsonContent);

        var result = await response.Content.ReadFromJsonAsync<Response<Guid>>();
        if (!response.IsSuccessStatusCode)
        {
            return new Response() { IsSuccess = false, Message = result.Message };
        }

        if (result == null)
            return new Response() { IsSuccess = false, Message = result.Message };

        //Dictionary<string, string>() { { "Error", "Failed" } };

        return new Response() { IsSuccess = true, Message = "Succes" };
    }

    public async Task<Response<LoginResponseModel>> Login(LoginModel loginModel)
    {
        var jsonContent = GetJson(loginModel);
        var httpResponse = await httpClient.PostAsync("api/auth/login", jsonContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = "Something went wrong. Please try again later.";
            return Response<LoginResponseModel>.Failure(errorMessage);
        }

        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<Response<LoginResponseModel>>(responseContent, _serializerOptions);

        //var response = await httpResponse.Content.ReadFromJsonAsync<Response<LoginResponseModel>>();

        if (response?.IsSuccess == true && response.Data is not null)
        {
            await customAuthenticationStateProvider.MarkUserAsAuthenticated(response.Data.TokenResponse);
            return Response<LoginResponseModel>.Success(response.Data);
        }

        return Response<LoginResponseModel>.Failure(response?.Message ?? "Unknown error");
    }

    public async Task LogOut()
    {
        await customAuthenticationStateProvider.MarkUserAsLoggedOut();
    }

    private static StringContent GetJson<T>(T model) =>
        new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

}

