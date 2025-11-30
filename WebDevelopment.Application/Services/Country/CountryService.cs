using WebDevelopment.Domain.Entities;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Enum;
using WebDevelopment.Shared.Interfaces;

namespace WebDevelopment.Application.Services.Country;

public class CountryService(AppDbContext _dbContext) : ICountryService
{
    public async Task AddCountry(CountryDto countryDto)
    {
        var country = new Domain.Entities.Country
        {
            PeopleCount = countryDto.PeopleCount,
            Rank = (Domain.Enums.RankEnum)countryDto.Rank,
            Continent = (Domain.Enums.ContinentEnum)countryDto.Continent,
            CountrySize = (Domain.Enums.CountrySizeEnum)countryDto.CountrySize,
            Name = countryDto.Name,
            IsIsand = countryDto.IsIsand,
        };
        await _dbContext.AddAsync(country);
        await _dbContext.SaveChangesAsync();
    }
}
