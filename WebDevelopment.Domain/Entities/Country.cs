using WebDevelopment.Domain.Enums;

namespace WebDevelopment.Domain.Entities;

public class Country : BaseEntity
{
    public int PeopleCount { get; set; }
    public string? Type { get; set; }
    public RankEnum? Rank { get; set; }
    public ContinentEnum? Continent { get; set; }
    public CountrySizeEnum? CountrySize { get; set; }
    public bool IsIsland { get; set; }
}
