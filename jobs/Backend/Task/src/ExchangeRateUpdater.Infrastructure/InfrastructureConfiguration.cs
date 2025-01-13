using ExchangeRateUpdater.Application.Contracts.Persistence;
using ExchangeRateUpdater.Infrastructure.Logging;
using ExchangeRateUpdater.Infrastructure.Repositories;
using ExchangeRateUpdater.Infrastructure.Resiliency;
using ExchangeRateUpdater.Infrastructure.Services.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<ICnbExchangeRateRepository, CnbExchangeRateRepository>();
            services.AddScoped<ICnbExchangeService, CnbExchangeService>();

            return services
                .AddHttpServices(configuration);
        }

        private static IServiceCollection AddHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            CheckRequired(configuration, "ExchangeRateProviders:Cnb:UrlBaseAPI");

            var cnbApiUrl = configuration.GetValue<string>("ExchangeRateProviders:Cnb:UrlBaseAPI");

            services
                .AddTransient<HttpClientDelegatingHandler>();

            services
                .AddHttpClient<ICnbExchangeService, CnbExchangeService>(c =>
                {
                    c.BaseAddress = new Uri(cnbApiUrl!);
                })
                .AddHttpMessageHandler<HttpClientDelegatingHandler>()
                .AddResilienceHandler("DefaultResiliencePolicy", (p, c) => ResiliencyPolicies.GetRetryAndCircuitBreakerPolicy(p));

            return services;
        }

        private static void CheckRequired(IConfiguration configuration, string configItem)
        {
            if (string.IsNullOrWhiteSpace(configuration[configItem]))
            {
                throw new ArgumentNullException($"Mandatory configuration: {configItem}");
            }
        }
    }
}
