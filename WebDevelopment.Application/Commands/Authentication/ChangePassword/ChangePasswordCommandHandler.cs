using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebDevelopment.Domain.Entities;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.ChangePassword
{
    public class ChangePasswordCommandHandler(
        UserManager<User> userManager,
        ILogger<ChangePasswordCommandHandler> logger
        ) : IRequestHandler<ChangePasswordCommand, Response>
    {
        public async Task<Response> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Handling change password request for user {request.UserId}");

                var user = await userManager.FindByIdAsync(request.UserId.ToString());

                if (user == null)
                    return new Response("User not found");

                if (request.ChangePasswordDto.NewPassword != request.ChangePasswordDto.ConfirmPassword)
                {
                    return new Response("New password and confirm password does not match.", false);
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var result = await userManager.ResetPasswordAsync(user, token, request.ChangePasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    logger.LogError($"Password change failed for user {request.UserId} error {result.Errors} ");
                    return new Response("Password change failed.", false);
                }

                user.IsFirstLogin = false;

                await userManager.UpdateAsync(user);
                logger.LogInformation($"Password changed successfully for user {request.UserId}");
                return new Response();

            }
            catch (Exception ex)
            {
                logger.LogError($"Password change failed for user {request.UserId} error: {ex} ");
                return new Response("Password change failed.", false);
            }
        }
    }
}
