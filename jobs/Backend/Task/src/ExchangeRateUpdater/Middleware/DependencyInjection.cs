using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Infrastructure.Middleware;
using ExchangeRateUpdater.Infrastructure.Providers.Middleware;

namespace ExchangeRateUpdater.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration? configuration = null)
    {
        // Register infrastructure services
        services.AddInfrastructure();
        services.AddThirdPartyProviders(configuration);
        
        return services;
    }
} 