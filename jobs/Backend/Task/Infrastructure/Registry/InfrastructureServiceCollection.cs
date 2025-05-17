using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Registry
{
    // The infrastrucutre folder could be (in a bigger project) a separate project.
    internal static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddExchangeRateUpdater(this IServiceCollection services, IConfiguration config)
            => services
                .AddCNBOptions(config)
                .AddSingleton<IExchangeRateProvider, CNBExchangeRateProvider>()
                .AddHttpClient<IExchangeRateProvider, CNBExchangeRateProvider>((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<CNBOptions>>()
                        .Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                })
            .Services;

        private static IServiceCollection AddCNBOptions(this IServiceCollection services, IConfiguration config) =>
            services.Configure<CNBOptions>(config.GetSection(key: nameof(CNBOptions)));
    }
}
