using System.ComponentModel.DataAnnotations;

namespace WebDevelopment.Client.Models.Authentication;
    
public class ChangePasswordModel
{
    [Required(ErrorMessageResourceName = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    //[Required(ErrorMessageResourceName = "Username is required")]
    //public string UserName { get; set; } = string.Empty;
}
