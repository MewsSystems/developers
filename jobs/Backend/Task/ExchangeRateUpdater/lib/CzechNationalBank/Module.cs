using System;
using System.Net.Http;
using System.Runtime;
using ExchangeRateUpdater.Lib.Shared;
using ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider;
using Microsoft.Extensions.DependencyInjection;

namespace V1CzechNationBankExchangeRateProvider.DependencyModule
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCzechNationBankExchangeRateProviderModule(
            this IServiceCollection services,
            ExchangeRateProviderSettings settings,
            bool usePipedOutput
        )
        {

            services.AddSingleton<IExchangeRateProviderSettings, ExchangeRateProviderSettings>(provider => settings);
            services.AddSingleton<ConsoleLoggerSettings, ConsoleLoggerSettings>(
                provider => new ConsoleLoggerSettings()
                {
                    Enabled = !usePipedOutput // avoid console logging when output is piped
                });

            services.AddSingleton<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
            services.AddSingleton<IExchangeRatesParallelHttpClient, ExchangeRatesParallelHttpClient>();
            services.AddSingleton<IExchangeRateHttpClient, ExchangeRateHttpClient>();
            services.AddSingleton<IFixedWindowRateLimiter, FixedWindowRateLimiter>();
            services.AddSingleton<FixedWindowRateLimiterOptions, FixedWindowRateLimiterOptions>(
                provider => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = settings.RateLimitCount,
                    Window = TimeSpan.FromSeconds(settings.RateLimitCount),
                    AutoReplenishment = true
                });

            services.AddSingleton<ILogger, ConsoleLogger>();

            // when more providers use this pattern we would need to add some extensions to allow injection of keyed instances of a service
            services.AddSingleton<HttpClient, HttpClient>(provider =>
            {
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = false
                };
                HttpClient httpClient = new HttpClient(handler);
                httpClient.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);

                return httpClient;
            });
            return services;
        }
    }
}