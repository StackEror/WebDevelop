using MediatR;
using WebDevelopment.Shared.DTOs.Authentication;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.UpdateAccesToken;

public record UpdateAccesTokenCommand() : IRequest<Response<LoginResponseDto>>;