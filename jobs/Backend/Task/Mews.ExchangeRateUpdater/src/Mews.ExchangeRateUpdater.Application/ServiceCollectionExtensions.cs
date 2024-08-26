using Mews.ExchangeRateUpdater.Application.ExchangeRates;
using Mews.ExchangeRateUpdater.Application.MapperProfiles;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRateUpdater.Application;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add business dependencies.
    /// </summary>
    /// <param name="services">DI collection services</param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        _ = services.AddAutoMapper(typeof(DefaultMapperProfile));

        // Add application services
        _ = services.AddScoped<IExchangeRateAppService, ExchangeRateAppService>();

        return services;
    }
}
