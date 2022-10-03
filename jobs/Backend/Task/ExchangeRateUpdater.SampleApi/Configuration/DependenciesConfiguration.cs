using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Providers.Providers;

namespace ExchangeRateUpdater.SampleApi.Configuration
{
    internal static class DependenciesConfiguration
    {
        internal static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services
                .AddHttpClient("httpClient");

            services
                .AddTransient<IExchangeRateProvider>(c =>
                new CzechNationalBankExchangeRateProvider(
                    c.GetRequiredService<IHttpClientFactory>().CreateClient("httpClient")));

            return services;
        }
    }
}
