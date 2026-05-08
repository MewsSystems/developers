using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.DependencyResolution
{
    public static class ConfigurationRegistrations
    {
        public static IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddOptions();

            var section = configuration.GetSection(ExchangeRateProviderConfigurationKeys.CnbApiClientConfiguration);
            var cnbApiConfig = section.Get<CnbApiClientConfiguration>();

            services.AddSingleton<CnbApiClientConfiguration>(cnbApiConfig);

            return services;
        }
    }
}
