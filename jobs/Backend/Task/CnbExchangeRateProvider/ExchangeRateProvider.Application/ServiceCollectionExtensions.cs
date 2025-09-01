namespace ExchangeRateProvider.Application;

using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.Providers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
// Register provider registry
services.AddSingleton<IProviderRegistry, ProviderRegistry>();

        // Register query handlers
        services.AddScoped<global::ExchangeRateProvider.Application.Queries.GetExchangeRatesQueryHandler>();

        return services;
    }
}
