using ExchangeRateProvider.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Application;

/// <summary>
/// Provides extension methods for configuring application layer services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application layer services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add application services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        _ = services.AddScoped<IExchangeRateProviderService, Services.ExchangeRateProviderService>();

        return services;
    }
}
