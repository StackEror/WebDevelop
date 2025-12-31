using MediatR;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetList
{
    public record GetCountriesListQuery() : IRequest<Response<List<CountryDto>>>;
}
