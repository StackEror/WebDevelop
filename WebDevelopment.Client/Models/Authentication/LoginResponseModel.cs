using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Models.Authentication;

public class LoginResponseModel
{
    public Guid UserId { get; set; } = Guid.Empty;
    public TokenResponse TokenResponse { get; set; }
    public bool IsFirstLogin { get; set; }
    public string Idnp { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
}