using ExchangeRateFinder.Application.Configuration;
using ExchangeRateUpdater.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateFinder.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddSingleton<IUpdateExchangeRateDateService, UpdateExchangeRateDateService>();
        }
    }
}
