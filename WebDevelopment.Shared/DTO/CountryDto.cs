using System.ComponentModel.DataAnnotations;
using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.DTO
{
    public class CountryDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? Description { get; set; } =  "TestDescription";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PeopleCount { get; set; }
        public string? Type { get; set; }
        [Required(ErrorMessage = "Rank is required")]
        public RankEnum? Rank { get; set; }
        [Required(ErrorMessage = "Continent is required")]
        public ContinentEnum? Continent { get; set; }
        [Required(ErrorMessage = "CountrySize is required")]
        public CountrySizeEnum? CountrySize { get; set; }
        public bool IsIsland { get; set; } = false;
    }
}
