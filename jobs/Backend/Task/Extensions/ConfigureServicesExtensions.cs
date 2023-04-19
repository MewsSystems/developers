using ExchangeRateProvider;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IMemoryCacheService, MemoryCacheService>()
                .AddScoped<IExchangeRateProviderService, ExchangeRateProviderService>();
        }
    }
}
