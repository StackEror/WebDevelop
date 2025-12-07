using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services
{
    public class CountryService(HttpClient _httpClient) : ICountryService
    {
        //public async Task AddCountry(CountryDto country)
        //{
        //    await _httpClient.PostAsJsonAsync("api/country/add", country);
        //}

        public async Task<Response> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/country/{id}");

            if (response.IsSuccessStatusCode)
                return new Response();
            else
                return new Response() { IsSuccess = false };
        }

        public async Task<Response<CountryDto>> GetById(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<Response<CountryDto>>($"api/country/{id}");

            if (response == null || response.Data == null)
                return new Response<CountryDto>(new()) { IsSuccess = false };
            else
                return new Response<CountryDto>(response.Data);
        }

        public async Task<Response<List<CountryDto>>> GetList()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountryDto>>("api/country");

            if (response != null)
                return new Response<List<CountryDto>>(response);
            else
                return new Response<List<CountryDto>>([]) { IsSuccess = false };
        }

        public async Task<Response> Update(CountryDto country)
        {
            var response = await _httpClient.PutAsJsonAsync("api/country", country);

            if (response.IsSuccessStatusCode)
                return new Response();
            else
                return new Response() { IsSuccess = false };
        }

        public async Task<Response<Guid>> Add(CountryDto country)
        {
            var response = await _httpClient.PostAsJsonAsync("api/country/add", country);

            var result = await response.Content.ReadFromJsonAsync<Guid>();

            return new Response<Guid>(result);
        }
    }
}
