using MediatR;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetById
{
    public record GetCountryByIdQuery(Guid id) : IRequest<Response<CountryDto>>;
}
