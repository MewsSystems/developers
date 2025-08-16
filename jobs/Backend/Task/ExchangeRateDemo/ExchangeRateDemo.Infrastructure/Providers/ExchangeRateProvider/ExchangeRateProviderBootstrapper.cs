using ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider
{
    [ExcludeFromCodeCoverage]
    public static class ExchangeRateProviderBootstrapper
    {
        public const string Name = "ExchangeRateProvider";

        public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExchangeRateProviderConfiguration>(configuration.GetSection(ExchangeRateProviderConfiguration.Name));
            services.AddRestProvider<IExchangeRateProvider, ExchangeRateProvider>(ConfigureClient);
            return services;
        }

        private static void ConfigureClient(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            var section = serviceProvider.GetRequiredService<IOptions<ExchangeRateProviderConfiguration>>().Value;
            httpClient.BaseAddress = section.Endpoint;
            httpClient.Timeout = section.Timeout;
        } 
    }
}
