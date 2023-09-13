using System.Diagnostics.CodeAnalysis;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Clients;
using ExchangeRateUpdater.Infrastructure.Providers;
using ExchangeRateUpdater.Infrastructure.Services;
using ExchangeRateUpdater.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddHttpClient<ICzechNationalBankApiClient, CzechNationalBankApiClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<CzechNationalBankApiSettings>>();
                client.BaseAddress = new Uri(options.Value.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.Value.TimeoutSec);
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
            services.AddScoped<IExchangeRateProviderFactory, ExchangeRateProviderFactory>();

            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            return services;
        }

        public static IServiceCollection ConfigureApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CzechNationalBankApiSettings>(configuration.GetSection("CzechNationalBankApi"));

            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache()
                .AddScoped<ICache, Cache>();

            return services;
        }

        public static IServiceCollection AddMonitoring(this IServiceCollection services)
        {
            services.AddTransient<IMonitorProvider, MonitorProvider>();

            return services;
        }
    }
}
