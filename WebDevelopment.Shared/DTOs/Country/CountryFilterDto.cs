using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.DTOs.Country;

public class CountryFilterDto
{
    public RankEnum? Rank { get; set; }
    public ContinentEnum? Continent { get; set; }
    public CountrySizeEnum? CountrySize { get; set; }
}
