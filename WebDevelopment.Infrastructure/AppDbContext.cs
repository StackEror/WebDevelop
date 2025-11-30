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
}
