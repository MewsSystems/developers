using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Middleware;

namespace ExchangeRateUpdater;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(IConfiguration? configuration = null)
    {
        var services = new ServiceCollection();

        services.AddScoped<ExchangeRateProvider>();
        // Register all application services using the modular DI approach
        services.AddApplicationServices(configuration);

        return services;
    }
} 