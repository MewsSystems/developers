using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core
{
    public static class CoreAdapter
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection service)
        {
            return service;
        }
    }
}
