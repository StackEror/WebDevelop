using Microsoft.AspNetCore.Identity;

namespace WebDevelopment.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Roles { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public bool IsFirstLogin { get; set; }
}
