using MediatR;
using WebDevelopment.Shared.DTOs.Country;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Country.Update;

public record UpdateCountryCommand(CountryDto country) : IRequest<Response>;
