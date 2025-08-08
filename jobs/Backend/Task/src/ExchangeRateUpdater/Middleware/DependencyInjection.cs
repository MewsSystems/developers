using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Infrastructure.Middleware;
using ExchangeRateUpdater.Infrastructure.Providers.Middleware;

namespace ExchangeRateUpdater.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration = null)
    {
        // We can use redis here
        services.AddDistributedMemoryCache();
        
        // Register infrastructure services with configuration
        services.AddInfrastructure(configuration);
        services.AddThirdPartyProviders(configuration);
        
        // Register application services
        services.AddScoped<ExchangeRateProvider>();
        
        return services;
    }
} 