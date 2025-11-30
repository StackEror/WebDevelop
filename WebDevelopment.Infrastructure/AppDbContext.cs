using Microsoft.EntityFrameworkCore;
using WebDevelopment.Domain.Entities;

namespace WebDevelopment.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
}
