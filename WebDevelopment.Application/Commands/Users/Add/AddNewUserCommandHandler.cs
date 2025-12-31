using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Security;
using WebDevelopment.Domain.Entities;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Users.Add;

public class AddNewUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ILogger<AddNewUserCommandHandler> logger,
    AppDbContext dbContext,
    ApplicationUserManager appUserManager
    ) : IRequestHandler<AddNewUserCommand, Response>
{
    public async Task<Response> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
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

            request.User.RoleId = await dbContext.Roles
                .Where(r => r.Name == "Client")
                .Select(n => n.Id.ToString())
                .FirstOrDefaultAsync();

            var result = await userManager.CreateAsync(newUser, request.User.Password);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return Response<Guid>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var role = await roleManager.FindByIdAsync(request.User.RoleId);

            if (role == null)
            {
                logger.LogError($"Role not found, role Id: {request.User.RoleId}");
                return Response<Guid>.Failure("Role not found");
            }

            var roleName = await dbContext.Roles
                .Where(r => r.Id == Guid.Parse(request.User.RoleId))
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            var resultAddRole = await userManager.AddToRoleAsync(newUser, roleName ?? string.Empty);

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
