using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Client.Models.Country
{
    public class CountryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PeopleCount { get; set; }
        public string? Type { get; set; }
        public RankEnum? Rank { get; set; }
        public ContinentEnum Continent { get; set; }
        public CountrySizeEnum? CountrySize { get; set; }
        public bool IsIsland { get; set; } 
    }
}
