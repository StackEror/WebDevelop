using WebDevelopment.Domain.Entities;
using WebDevelopment.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebDevelopment.Application.Security;

public class ApplicationUserManager(
    IUserStore<User> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<User> passwordHasher,
    IEnumerable<IUserValidator<User>> userValidators,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<User>> logger,
    RoleManager<IdentityRole<Guid>> roleManager,
    AppDbContext dbContext
    ) : UserManager<User>(
    store, optionsAccessor, passwordHasher, userValidators,
    passwordValidators, keyNormalizer, errors, services, logger)
{

}
