using System.ComponentModel.DataAnnotations;

namespace WebDevelopment.Shared.Enums;

public enum CountrySizeEnum
{
    [Display(Name = "Small")]
    Small = 0,
    [Display(Name = "Medium")]
    Medium = 1,
    Large = 2,
}
