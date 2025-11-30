using WebDevelopment.Domain.Enums;

namespace WebDevelopment.Domain.Entities;

public class Country : BaseEntity
{
    public int PeopleCount { get; set; }
    public string? Type { get; set; }
    public Rank? Rank { get; set; }
    public Continent? Continent { get; set; }
    public CountrySize? CountrySize { get; set; }
    public bool IsIsand { get; set; }
}
