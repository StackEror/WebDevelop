using MediatR;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetById
{
    public record GetCountryByIdQuery(Guid id) : IRequest<Response<CountryDto>>;
}
