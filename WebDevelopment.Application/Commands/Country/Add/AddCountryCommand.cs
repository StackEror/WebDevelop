using MediatR;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Country.Add
{
    public record AddCountryCommand(CountryDto country) : IRequest<Response<Guid>>;
}
