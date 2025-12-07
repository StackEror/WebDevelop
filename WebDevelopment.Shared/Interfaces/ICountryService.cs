using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Shared.Interfaces;

public interface ICountryService
{
    Task<Response<Guid>> Add(CountryDto country);
    Task<Response> Update(CountryDto country);
    Task<Response<CountryDto>> GetById(Guid id);
    Task<Response<List<CountryDto>>> GetList();
    Task<Response> Delete(Guid id);
}
