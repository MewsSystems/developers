using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Repositories;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration? configuration = null)
    {
        // Register cache services
        services.AddScoped<ICacheService, DistributedCacheService>();
        
        // Register configuration
        if (configuration != null)
        {
            var config = new ExchangeRateProvidersConfig();
            configuration.GetSection("ExchangeRateProviders").Bind(config);
            services.AddSingleton(Options.Create(config));
        }
        else
        {
            // Register default configuration
            var defaultConfig = new ExchangeRateProvidersConfig();
            services.AddSingleton(Options.Create(defaultConfig));
        }
        
        // Register repositories - DI container will automatically inject all IExchangeRateProvider implementations
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        
        return services;
    }
} 