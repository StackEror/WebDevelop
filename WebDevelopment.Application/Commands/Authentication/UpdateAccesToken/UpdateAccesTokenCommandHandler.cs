using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Security;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.DTOs.Authentication;
using WebDevelopment.Shared.Helpers;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.UpdateAccesToken;

public class UpdateAccesTokenCommandHandler(
    UserContext userContext,
    AppDbContext dbContext,
    ApplicationUserManager userManager,
    JwtHelper jwtHelper,
    ILogger<UpdateAccesTokenCommandHandler> logger
    ) : IRequestHandler<UpdateAccesTokenCommand, Response<LoginResponseDto>>
{
    public async Task<Response<LoginResponseDto>> Handle(UpdateAccesTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = userContext.GetId();

            if (userId == null || userId == Guid.Empty)
            {
                return Response<LoginResponseDto>.Failure("User not found");
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                return Response<LoginResponseDto>.Failure("User not found");
            }

            if (user.IsActive)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    IsFirstLogin = user.IsFirstLogin,
                    Email = user.Email,
                    //Roles = user.Roles.Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToList()
                };

                var token = jwtHelper.GenerateToken(userDto);
                var refreshToken = jwtHelper.GenerateRefreshToken(userDto);

                var response = new LoginResponseDto
                {
                    IsFirstLogin = user.IsFirstLogin,
                    TokenResponse = new TokenResponse(token, refreshToken)
                };

                return Response<LoginResponseDto>.Success(response);
            }

            return Response<LoginResponseDto>.Failure("Invalid username or password");
        }
        catch (Exception ex)
        {
            logger.LogError($"{ex}, Error occurred while processing login request.");
            return Response<LoginResponseDto>.Failure("Error occurred while processing login request.");
        }
    }
}
