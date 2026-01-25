using System.Net;
using WebDevelopment.Shared.DTOs.Country;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Page;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services;

public class CountryService(
    ApiClient apiClient
    ) : ICountryService
{
    //public async Task AddCountry(CountryDto country)
    //{
    //    await _httpClient.PostAsJsonAsync("api/country/add", country);
    //}

    public async Task<Response<Guid>> Add(CountryDto country)
    {
        var response = await apiClient.PostAsJsonGetResponseAsync("api/country/add", country);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await response.Content.ReadFromJsonAsync<Guid>();
            return new Response<Guid>(result);
        }
        else
        {
            return new Response<Guid>(Guid.Empty) { IsSuccess = false };
        }
    }

    public async Task<Response> Update(CountryDto country)
    {
        var response = await apiClient.PutAsync("api/country", country);

        return response.IsSuccess ?
            new Response() :
            response;
    }

    public async Task<Response<PaginatedCollection<CountryDto>>> GetList(PageFilter<CountryFilterDto> PageFilter)
    {
        var response = await apiClient.GetFromJsonAsync<Response<PaginatedCollection<CountryDto>>>(
            $"api/country?" +
            $"searchKeyword={PageFilter.SearchKeyword}&" +
            $"pageNumber={PageFilter.PageNumber}&" +
            $"pageSize={PageFilter.PageSize}&" +
            $"sortDirection={PageFilter.PageSize}&" +
            $"sortColumn={PageFilter.PageSize}&" +
            $"filter={PageFilter.Filter}");

        if (response != null)
            return new Response<PaginatedCollection<CountryDto>>(response.Data);
        else
            return new Response<PaginatedCollection<CountryDto>>(new PaginatedCollection<CountryDto>()) { IsSuccess = false };
    }

    public async Task<Response<CountryDto>> GetById(Guid id)
    {
        var response = await apiClient.GetFromJsonAsync<Response<CountryDto>>($"api/country/{id}");

        if (response == null || response.Data == null)
            return new Response<CountryDto>(new()) { IsSuccess = false };
        else
            return new Response<CountryDto>(response.Data);
    }

    public async Task<Response> Delete(Guid id)
    {
        var response = await apiClient.DeleteAsync($"api/country/{id}");

        return response.IsSuccess ?
            new Response() :
            new Response() { IsSuccess = false };
    }
}
