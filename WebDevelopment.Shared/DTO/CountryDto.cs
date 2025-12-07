using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.DTO
{
    public class CountryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "string.Empty";
        public string? Description { get; set; } =  "TestDescription";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int PeopleCount { get; set; } = 100;
        public string? Type { get; set; } = "TestType";
        public RankEnum? Rank { get; set; } = RankEnum.FirstCountry;
        public ContinentEnum? Continent { get; set; } = ContinentEnum.Antarctica;
        public CountrySizeEnum? CountrySize { get; set; } = CountrySizeEnum.Medium;
        public bool IsIsland { get; set; } = false;
    }
}
