namespace WebDevelopment.Application.DTOs;

public class NewUserDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? Email { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public string Password { get; set; }
    public bool IsActive { get; set; } = true;
}
