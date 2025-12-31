using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebDevelopment.Shared.Interfaces.Authentication;

namespace WebDevelopment.Application.Security;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IEnumerable<Claim> UserClaims = httpContextAccessor?.HttpContext?.User.Claims ?? [];

    public Guid? GetId()
    {
        var userIdClaim = UserClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim?.Value, out var guidId) ? guidId : null;
    }

    public IEnumerable<string> GetRoles()
    {
        return UserClaims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value);
    }
}
