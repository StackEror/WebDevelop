using Microsoft.EntityFrameworkCore;
using WebDevelopment.Domain.Entities;

namespace WebDevelopment.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
    {
    }
    public DbSet<Country> Countries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Domain.Entities.File> Files { get; set; }
}
