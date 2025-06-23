using ExchangeRateUpdater.Infrastructure.cnbConector;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Service.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient<ICnbService, CnbService>();

            services.Configure<CnbApiOptions>(
                configuration.GetSection("CnbApi"));

            return services;
        }
    }
}
