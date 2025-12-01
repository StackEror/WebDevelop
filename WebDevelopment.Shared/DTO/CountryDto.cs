using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.DTO
{
    public class CountryDto
    {
        public int PeopleCount { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public RankEnum? Rank { get; set; }
        public ContinentEnum Continent { get; set; }
        public CountrySizeEnum? CountrySize { get; set; }
        public bool IsIsland { get; set; }
    }
}
