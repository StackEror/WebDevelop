using WebDevelopment.Shared.DTOs.Country;
using WebDevelopment.Shared.DTOs.Page;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Shared.Interfaces;

public interface ICountryService
{
    Task<Response<Guid>> Add(CountryDto country);
    Task<Response> Update(CountryDto country);
    Task<Response<CountryDto>> GetById(Guid id);
    Task<Response<PaginatedCollection<CountryDto>>> GetList(PageFilter<CountryFilterDto>? PageFilter);
    Task<Response> Delete(Guid id);
}
