using WebDevelopment.Application.Interfaces.Caching;
using WebDevelopment.Shared.DTOs.Country;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetById;

public sealed record GetCountryByIdQuery(Guid id) : ICachedQuery<Response<CountryDto>>
{
    public string CacheKey => $"country-{id}";
    public TimeSpan? Expiration => null;
}
