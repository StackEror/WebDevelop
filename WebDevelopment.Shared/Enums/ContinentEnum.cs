using System.ComponentModel.DataAnnotations;

namespace WebDevelopment.Shared.Enums;

public enum ContinentEnum
{
    [Display(Name = "Europe")]
    Europe = 0,

    [Display(Name = "SouthAmerica")]
    SouthAmerica = 1,

    [Display(Name = "NorthAmerica")]
    NorthAmerica = 2,

    [Display(Name = "Africa")]
    Africa = 3,

    [Display(Name = "Australia")]
    Australia = 4,

    [Display(Name = "Antarctica")]
    Antarctica = 5
}