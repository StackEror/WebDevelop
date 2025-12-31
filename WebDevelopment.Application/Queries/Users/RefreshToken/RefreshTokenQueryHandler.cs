using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using WebDevelopment.Application.Security;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Helpers;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Users.RefreshToken;

public class RefreshTokenQueryHandler(
    ApplicationUserManager userManager,
    JwtHelper jwtHelper,
    ILogger<RefreshTokenQueryHandler> logger
    ) : IRequestHandler<RefreshTokenQuery, Response<string>>
{
    public async Task<Response<string>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var principal = jwtHelper.ValidateRefreshToken(request.RefreshToken);

            if (principal == null)
            {
                return Response<string>.Failure("Invalid or expired refresh token");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Response<string>.Failure("User ID not found in token");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null || !user.IsActive)
            {
                return Response<string>.Failure("User not found or inactive");
            }

            var roles = await userManager.GetRolesAsync(user);
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email ?? string.Empty,
                IsFirstLogin = user.IsFirstLogin,
                Roles = [..roles]
            };

            var newAccessToken = jwtHelper.GenerateToken(userDto);

            return Response<string>.Success(newAccessToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while refreshing token.");
            return Response<string>.Failure("Failed to refresh token.");
        }
    }
}
