using MediatR;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Users.RefreshToken;

public record RefreshTokenQuery(string RefreshToken) : IRequest<Response<string>>;