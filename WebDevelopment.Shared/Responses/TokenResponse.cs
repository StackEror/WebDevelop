namespace WebDevelopment.Shared.Responses;

public class TokenResponse(string token, string refreshToken)
{
    public string Token { get; set; } = token;
    public string RefreshToken { get; set; } = refreshToken;
}
