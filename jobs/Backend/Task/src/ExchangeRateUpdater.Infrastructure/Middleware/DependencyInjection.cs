using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Domain.Providers;

namespace ExchangeRateUpdater.Infrastructure.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register repositories with factory pattern to inject all IExchangeRateProvider implementations
        services.AddScoped<IExchangeRateRepository>(serviceProvider =>
        {
            var exchangeRateProviders = serviceProvider.GetServices<IExchangeRateProvider>().ToArray();
            return new ExchangeRateRepository(exchangeRateProviders);
        });
        
        return services;
    }
} 