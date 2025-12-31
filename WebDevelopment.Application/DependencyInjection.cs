using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebDevelopment.Application.Behaviours;
using WebDevelopment.Application.Security;
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

        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserContext, UserContext>();


        return services;
    }
}
