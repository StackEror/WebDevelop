using MediatR;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetList
{
    public record GetCountriesListQuery() : IRequest<Response<List<CountryDto>>>;
}
