using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Interfaces;

namespace WebDevelopment.Client.Services
{
    public class CountryService(HttpClient _httpClient) : ICountryService
    {
        public async Task AddCountry(CountryDto country)
        {
            await _httpClient.PostAsJsonAsync("api/country/add", country);
        }
    }
}
