namespace WebDevelopment.Application.DTOs;

public class NewUserDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? Email { get; set; }
    public List<string> RoleIds { get; set; } = [];
    public string Password { get; set; }
    public bool IsActive { get; set; } = true;
}
