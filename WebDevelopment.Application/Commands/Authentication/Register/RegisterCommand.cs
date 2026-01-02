using MediatR;
using WebDevelopment.Shared.DTOs.Authentication;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.Register;

public record RegisterCommand(RegisterUserDto User) : IRequest<Response>;