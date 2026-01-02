using MediatR;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Users.RefreshToken;

public record RefreshTokenQuery(RefreshTokenDto RefreshToken) : IRequest<Response<string>>;