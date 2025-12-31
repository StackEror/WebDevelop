using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebDevelopment.Infrastructure;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables();

var configuration = configurationBuilder.Build();
var connectionString = configuration.GetConnectionString("Default") ?? string.Empty;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(connectionString);

try
{
    Console.WriteLine($"Migrator database {connectionString}");
    using AppDbContext sc = new AppDbContext(optionsBuilder.Options);
    Console.WriteLine("Starting migration");
    sc.Database.Migrate();
    new DbSeedingInitializer(sc).Seed();
    Console.WriteLine("End migration");
}
catch (Exception ex)
{
    Console.WriteLine($"Migration failed: {ex.Message}");
    throw;
}


public class DbSeedingInitializer(AppDbContext dbContext)
{
    public void Seed()
    {
        AddRolesIfDoesntExists();
        dbContext.SaveChanges();
    }

    private void AddRolesIfDoesntExists()
    {
        var role1 = dbContext.Roles.FirstOrDefault(r => r.Name == "Client");
        if (role1 == null)
        {
            dbContext.Add(new IdentityRole<Guid>("Client")
            {
                Id = Guid.NewGuid(),
            });
        }
        var role2 = dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (role2 == null)
        {
            dbContext.Add(new IdentityRole<Guid>("Admin")
            {
                Id = Guid.NewGuid(),
            });
        }
    }
}