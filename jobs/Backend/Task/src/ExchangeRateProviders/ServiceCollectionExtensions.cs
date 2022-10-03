using ExchangeRateUpdater.Providers.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace ExchangeRateUpdater.Providers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRateProviders(this IServiceCollection services)
        {
            services
                .AddHttpClient("httpClient");

            services
                .AddScoped<IExchangeRateProvider>(c =>
                new CzechNationalBankExchangeRateProvider(
                    c.GetRequiredService<IHttpClientFactory>().CreateClient("httpClient")));

            return services;
        }
    }
}
