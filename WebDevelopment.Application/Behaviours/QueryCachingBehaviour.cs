using MediatR;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Interfaces.Caching;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Behaviours;

internal sealed class QueryCachingBehaviour<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<QueryCachingBehaviour<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TResponse>
    where TResponse : Response

{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse? cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

        string name = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            logger.LogInformation($"Cache hit for {name}");

            return cachedResult;
        }

        logger.LogInformation($"Cache miss for {name}");

        var result = await next();

        if (result.IsSuccess)
        {
            await cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
        }

        return result;
    }
}
