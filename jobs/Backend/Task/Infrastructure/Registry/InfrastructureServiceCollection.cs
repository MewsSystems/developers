using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure.Registry
{
    // The infrastrucutre folder could be (in a bigger project) a separate project.
    internal static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddExchangeRateUpdater(this IServiceCollection services, IConfiguration config)
            => services
                .AddCNBOptions(config)
                .AddSingleton<IExchangeRateProvider, CNBExchangeRateProvider>();

        private static IServiceCollection AddCNBOptions(this IServiceCollection services, IConfiguration config) =>
            services.Configure<CNBOptions>(config.GetSection(key: nameof(CNBOptions)));
    }
}
