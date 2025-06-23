using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Infra.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.SetupHttpClient(configuration);
    }
}