using System.ComponentModel.DataAnnotations;

namespace WebDevelopment.Client.Models.Authentication;

public class RegisterUserModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public bool IsActive { get; set; } = true;
}
