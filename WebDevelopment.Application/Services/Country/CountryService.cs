using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

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
            Type = countryDto.Type,
            IsIsland = countryDto.IsIsland,
        };
        await _dbContext.AddAsync(country);
        await _dbContext.SaveChangesAsync();
    }

    public Task<Response> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Response<CountryDto>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<CountryDto>>> GetList()
    {
        throw new NotImplementedException();
    }

    public Task<Response> Update(CountryDto country)
    {
        throw new NotImplementedException();
    }

    Task<Response<Guid>> ICountryService.AddCountry(CountryDto country)
    {
        throw new NotImplementedException();
    }
}
