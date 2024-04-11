using MewsFinance.Application.DependencyInjection;
using MewsFinance.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Startup
    {
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInfrastructure();
            serviceCollection.AddApplicationLayer();
        }
    }
}
