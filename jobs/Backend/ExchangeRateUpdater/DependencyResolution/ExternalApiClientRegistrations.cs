using ExchangeRateUpdater.ClientFactories;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.DependencyResolution
{
    public static class ExternalApiClientRegistrations
    {
        public static IServiceCollection AddExternalApiRegistrations(this IServiceCollection services)
        {
            // named http client to be decided
            //services.AddHttpClient("CNB", httpClient =>
            //{
            //    // config
            //});

            // HTTP CLIENT FACTORY
            services.AddHttpClient();
            services.AddTransient<ICnbApiClientFactory, CnbApiClientFactory>();
            services.AddSingleton(s => s.GetRequiredService<ICnbApiClientFactory>().CreateClient());

            return services;
        }
    }
}
