using WebDevelopment.Shared.DTO;

namespace WebDevelopment.Shared.Interfaces;

public interface ICountryService
{
    Task AddCountry(CountryDto country);
}
