using ExchangeRateUpdater.Application.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            return services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        }
    }
}