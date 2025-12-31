using MediatR;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Shared.DTOs.Authentication;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.Login;

public record LoginCommand(UserLoginDto UserLoginDto) : IRequest<Response<LoginResponseDto>>;