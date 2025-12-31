namespace WebDevelopment.Shared.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; } = [];
    public string UserName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public string? Nationality { get; set; }
    public bool IsFirstLogin { get; set; }
}
