using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebDevelopment.Application.Behaviours;
using WebDevelopment.Application.Interfaces.Caching;
using WebDevelopment.Application.Security;
using WebDevelopment.Application.Services;
using WebDevelopment.Application.Services.Caching;
using WebDevelopment.Application.Services.Country;
using WebDevelopment.Application.Services.File;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Interfaces.Authentication;

namespace WebDevelopment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehaviour<,>));

        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped(typeof(IDataRequestHandlerService<,>), typeof(DataRequestHandlerService<,>));
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
