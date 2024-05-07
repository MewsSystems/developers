using ExchangeRateFinder.Application.Configuration;
using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateFinder.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddTransient<IExchangeRateParser, ExchangeRateParser>();
            services.AddTransient<IUpdateCZKExchangeRateDataService, UpdateCZKExchangeRateDataService>();

            services.Configure<CBNConfiguration>(configuration.GetSection(nameof(CBNConfiguration)));
        }
    }
}
