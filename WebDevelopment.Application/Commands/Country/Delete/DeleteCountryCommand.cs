using MediatR;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Country.Delete
{
    public record DeleteCountryCommand(Guid id) : IRequest<Response>;
}
