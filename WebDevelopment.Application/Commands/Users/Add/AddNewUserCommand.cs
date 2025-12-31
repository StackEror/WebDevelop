using MediatR;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Users.Add;

public record AddNewUserCommand(NewUserDto User) : IRequest<Response>;