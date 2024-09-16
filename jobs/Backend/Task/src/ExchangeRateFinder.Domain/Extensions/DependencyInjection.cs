using ExchangeRateFinder.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateFinder.Domain.Extensions
{
    public static class DependencyInjection
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddTransient<IExchangeRateCalculator, ExchangeRateCalculator>();
        }
    }
}
