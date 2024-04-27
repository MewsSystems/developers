using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Application;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateFinder.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddTransient<IExchangeRateParser, ExchangeRateParser>();
            services.AddTransient<IUpdateCZKExchangeRateDataService, UpdateCZKExchangeRateDataService>();
        }
    }
}
