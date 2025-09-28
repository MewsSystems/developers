using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure.Installers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration,
        bool useApiCache = true)
    {
        services
            .AddExchangeRateServices(configuration)
            .AddCaching(configuration, useApiCache);

        return services;
    }
}