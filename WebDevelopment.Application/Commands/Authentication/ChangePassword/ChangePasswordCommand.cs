using MediatR;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Authentication.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto ChangePasswordDto, Guid UserId) : IRequest<Response>;