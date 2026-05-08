using ExchangeRateProvider.Domain.Constants;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Infrastructure.ExternalServices.CZK;
using ExchangeRateProvider.Infrastructure.Factories;
using ExchangeRateProvider.Infrastructure.Policies;
using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Infrastructure;

[ExcludeFromCodeCoverage(Justification = "Dependency injection configuration - no testable logic, verified through integration tests")]
public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IExchangeRateServiceFactory, ExchangeRateServiceFactory>();
        services.AddKeyedTransient<IExchangeRateService, CzkExchangeRateService>(CurrencyServiceKeys.CZK);

        services.AddTransient<ICzkApiClient, CzkApiClient>();
        services.AddTransient<ICzkExchangeRateMapper, CzkExchangeRateMapper>();

        services.AddLazyCache();

        ConfigurePolicyRegistry(services);
        ConfigureHttpClients(services);

        return services;
    }

    private static void ConfigurePolicyRegistry(IServiceCollection services)
    {
        var registry = services.AddPolicyRegistry();
        registry.AddBasicRetryPolicy();
    }

    private static void ConfigureHttpClients(IServiceCollection services)
    {
        services.AddHttpClient<ICzkApiClient, CzkApiClient>();
    }
}
