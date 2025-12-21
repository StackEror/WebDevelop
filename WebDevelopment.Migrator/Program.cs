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
    Console.WriteLine("End migration");
}
catch (Exception ex)
{
    Console.WriteLine($"Migration failed: {ex.Message}");
    throw;
}