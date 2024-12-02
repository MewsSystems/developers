using ExchangeRateUpdater.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core
{
    public static class CoreAdapter
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection service, AppConfiguration appConfiguration)
        {
            service.AddSingleton(appConfiguration);

            return service;
        }
    }
}
