using MediatR;

namespace WebDevelopment.Application.Interfaces.Caching;

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
