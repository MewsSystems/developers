using ExchangeRateFinder.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateSyncService.Extensions
{
    public static class DependencyInjection
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddTransient<IExchangeRateCalculator, ExchangeRateCalculator>();
        }
    }
}
