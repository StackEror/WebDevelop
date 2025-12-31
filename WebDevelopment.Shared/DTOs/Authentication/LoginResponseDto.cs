using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Shared.DTOs.Authentication;

public class LoginResponseDto
{
    public Guid UserId { get; set; } = Guid.Empty;

    public TokenResponse? TokenResponse { get; set; }

    public bool IsFirstLogin { get; set; }
}
