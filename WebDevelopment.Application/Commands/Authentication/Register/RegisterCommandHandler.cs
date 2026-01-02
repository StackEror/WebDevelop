using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Security;
using WebDevelopment.Domain.Entities;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.Register;

public class RegisterCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ILogger<RegisterCommandHandler> logger,
    AppDbContext dbContext,
    ApplicationUserManager appUserManager
    ) : IRequestHandler<RegisterCommand, Response>
{
    public async Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.User.Name,
                Surname = request.User.Surname,
                UserName = request.User.UserName,
                Email = request.User.Email,
                IsActive = true,
                Roles = "Client",
            };

            var dbRole = roleManager.Roles.FirstOrDefault(r => r.Name == "Client");
            var userRoleId = dbRole.Id.ToString();

            var result = await userManager.CreateAsync(newUser, request.User.Password);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to register user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return Response<Guid>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var role = await roleManager.FindByIdAsync(userRoleId);

            if (role == null)
            {
                logger.LogError($"Role not found, role Id: {userRoleId}");
                return Response<Guid>.Failure("Role not found");
            }

            var resultAddRole = await userManager.AddToRoleAsync(newUser, dbRole.Name ?? string.Empty);

            if (!resultAddRole.Succeeded)
            {
                logger.LogError("Failed to add role to user: {Errors}", string.Join(", ", resultAddRole.Errors.Select(e => e.Description)));
                return Response<Guid>.Failure(string.Join(", ", resultAddRole.Errors.Select(e => e.Description)));
            }

            logger.LogInformation($"User created successfully with ID: {newUser.Id}");
            return Response<Guid>.Success(newUser.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a new user");
            return Response<Guid>.Failure("An error occurred while creating a new user");
        }
    }
}
