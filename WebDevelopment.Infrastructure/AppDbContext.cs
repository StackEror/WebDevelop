using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDevelopment.Domain.Entities;

namespace WebDevelopment.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<User> AspNetUsers { get; set; }
    public DbSet<Domain.Entities.File> Files { get; set; }
}
