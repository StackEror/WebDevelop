using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebDevelopment.Client.Models.Authentication;
using WebDevelopment.Client.StringConstants;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services;

public class CustomAuthenticationStateProvider(
    ProtectedLocalStorageService protectedLocalStorage,
    HttpClient httpClient,
    IConfiguration configuration,
    NavigationManager navigationManager
    ) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    private static long? GetExpirationDate(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        if (!jwtToken.Payload.TryGetValue("exp", out var expClaim))
            return null;

        var expSeconds = Convert.ToInt64(expClaim);

        return expSeconds;
    }

    private async Task<bool> VerificationUserToken()
    {
        var accesToken = await protectedLocalStorage.GetItemAsync<string>(AuthConstants.AccessToken);
        var refreshToken = await protectedLocalStorage.GetItemAsync<string>(AuthConstants.RefreshToken);

        if (string.IsNullOrEmpty(accesToken) || string.IsNullOrEmpty(refreshToken))
        {
            await MarkUserAsLoggedOut();
            return false;
        }

        var expirationDate = GetExpirationDate(accesToken!);
        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (expirationDate!.Value < currentUnixTime)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/auth/refresh", new RefreshTokenModel { RefreshToken = refreshToken });

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<string>>(responseBody);


                    if (result != null && !string.IsNullOrEmpty(result.Data))
                    {
                        await MarkUserAsAuthenticated(new TokenResponse(result.Data, refreshToken));
                        return true;
                    }
                }

                await MarkUserAsLoggedOut();
                return false;
            }
            catch
            {
                await MarkUserAsLoggedOut();
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public async Task MarkUserAsLoggedOut()
    {
        await protectedLocalStorage.RemoveItemAsync(AuthConstants.AccessToken);
        await protectedLocalStorage.RemoveItemAsync(AuthConstants.RefreshToken);

        //var identity = new ClaimsIdentity();
        //var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));

        navigationManager.NavigateTo("/login");
    }

    public async Task MarkUserAsAuthenticated(TokenResponse tokenResponse)
    {
        await protectedLocalStorage.SetItemAsync<string>(AuthConstants.AccessToken, tokenResponse.Token);
        await protectedLocalStorage.SetItemAsync<string>(AuthConstants.RefreshToken, tokenResponse.RefreshToken);

        var claims = ParseClaimsFromJwt(tokenResponse.Token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task<AuthenticatedUser?> GetAuthenticatedUser()
    {
        var token = await protectedLocalStorage.GetItemAsync<string>(AuthConstants.AccessToken);
        if(string.IsNullOrEmpty(token))
        {
            await protectedLocalStorage.RemoveItemAsync(AuthConstants.AccessToken);
            return null;
        }

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return await VerificationUserToken() ? ParseUserFromClaimsIdentity(identity) : null;     
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await protectedLocalStorage.GetItemAsync<string>(AuthConstants.AccessToken);
            if (string.IsNullOrEmpty(token))
            {
                await protectedLocalStorage.RemoveItemAsync(AuthConstants.AccessToken);
                return new AuthenticationState(_anonymous);
            }

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            var isValid = await VerificationUserToken();

            if (isValid)
                return new AuthenticationState(user);

            return new AuthenticationState(_anonymous);
        }
        catch
        {
            await MarkUserAsLoggedOut();
            return new AuthenticationState(_anonymous);
        }
    }

    public void AuthenticateUser(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private static List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(jwt) is JwtSecurityToken jsonToken)
        {
            claims.AddRange(jsonToken.Claims);
        }

        return claims;
    }

    private static AuthenticatedUser ParseUserFromClaimsIdentity(ClaimsIdentity identity)
    {
        if (identity == null)
            throw new ArgumentNullException(nameof(identity));

        var roles = identity.FindAll(ClaimTypes.Role).Select(roleCLaim => roleCLaim.Value).ToArray();

        var authenticatedUser = new AuthenticatedUser(
            Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            identity.FindFirst(ClaimTypes.Email)!.Value,
            identity.FindFirst(ClaimTypes.Name)!.Value,
            identity.FindFirst(ClaimTypes.Surname)!.Value,
            identity.FindFirst("IsFirstLogin")!.Value,
            roles
        );

        return authenticatedUser;
    }
}