using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace WebDevelopment.Application.Queries.Users.GetAllRoles;

public class GetAllRolesQueryHandler(
    RoleManager<IdentityRole<Guid>> roleManager,
    ILogger<GetAllRolesQueryHandler> logger
    ) : IRequestHandler<GetAllRolesQuery, Dictionary<string, string>>
{
    public async Task<Dictionary<string, string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userRoles = roleManager.Roles.ToList();

            var roles = userRoles.ToDictionary(r => r.Id.ToString(), r => r.Name);

            if (roles == null)
                return [];

            return roles;
        }
        catch (Exception ex)
        {
            logger.LogError("GetAllRolesQuery failed: {Exception}", ex);

            return [];
        }
    }
}
