using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace ExchangeRateUpdater.Providers
{
    [ExcludeFromCodeCoverage]
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