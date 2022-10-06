using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace ExchangeRateUpdater.Providers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRateProviders(this IServiceCollection services)
        {
            services
                .AddScoped<IExchangeRateProviderStrategyFactory>(c =>
                new ExchangeRateProviderStrategyFactory(
                    c.GetRequiredService<IHttpClientFactory>()));

            return services;
        }
    }
}
