using ExchangeRateUpdater.Infrastucture.Data.API.Abstractions;
using ExchangeRateUpdater.Infrastucture.Data.API.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddExchangeRateUpdaterInfrastuctureServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IExchangeRatesDailyAPIService, ExchangeRatesDailyAPIService>();
            services.AddTransient<IExternalAPIService, ExternalAPIService>();
            return services;
        }
    }
}
