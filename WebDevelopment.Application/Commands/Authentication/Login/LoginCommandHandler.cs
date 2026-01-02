using MediatR;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Security;
using WebDevelopment.Shared.DTOs.Authentication;
using WebDevelopment.Shared.DTOs.Users;
using WebDevelopment.Shared.Helpers;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.Login;

public class LoginCommandHandler(
    ApplicationUserManager userManager,
    JwtHelper jwtHelper,
    ILogger<LoginCommandHandler> logger
    ) : IRequestHandler<LoginCommand, Response<LoginResponseDto>>
{
    public async Task<Response<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByNameAsync(request.UserLoginDto.UserName);

            if (user == null || !await userManager.CheckPasswordAsync(user, request.UserLoginDto.Password))
            {
                return Response<LoginResponseDto>.Failure("Invalid username or password");
            }

            if (user.UserName != request.UserLoginDto.UserName)
            {
                return Response<LoginResponseDto>.Failure("Invalid username or password");
            }

            if (user.IsActive)
            {
                var roles = await userManager.GetRolesAsync(user);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    IsFirstLogin = user.IsFirstLogin,
                    Email = user.Email,
                    Roles = [..roles]
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
