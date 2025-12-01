using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services
{
    public class CountryService(HttpClient _httpClient) : ICountryService
    {
        public async Task AddCountry(CountryDto country)
        {
            await _httpClient.PostAsJsonAsync("api/country/add", country);
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
}
