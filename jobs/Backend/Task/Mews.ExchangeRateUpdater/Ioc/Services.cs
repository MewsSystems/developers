using Mews.ExchangeRateUpdater.Services;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB;
using Mews.ExchangeRateUpdater.Services.Infrastructure;
using Mews.ExchangeRateUpdater.Services.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRateUpdater.Ioc
{
    public static class Services
    {
        public static IServiceProvider Register(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("Properties\\appSettings.json", optional: false, reloadOnChange: true).Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<IRequestValidator, CurrencyCodesValidator>();

            services.AddHttpClient<IRestClient, RestClient>();

            services.AddScoped<IExchangeRateProviderResolver, ExchangeRateProviderResolver>();
            services.AddScoped<IExchangeRateProvider, CNBExchangeRatesProvider>();

            services.AddScoped<IExchangeRateProviderService, ExchangeRateProviderService>();

            return services.BuildServiceProvider();
        }
    }
}
