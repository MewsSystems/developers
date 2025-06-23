using ExchangeRateUpdater.ClientFactories;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.DependencyResolution
{
    public static class ExternalApiClientRegistrations
    {
        public static IServiceCollection AddExternalApiRegistrations(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<ICnbApiClientFactory, CnbApiClientFactory>();
            services.AddSingleton(s => s.GetRequiredService<ICnbApiClientFactory>().CreateCnbApiClient());

            return services;
        }
    }
}
