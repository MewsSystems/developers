using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using ExchangeRateUpdater.Common.Extensions;
using ExchangeRateUpdater.Common.Constants;
using ExchangeRateUpdater.ExchangeRate.Providers;
using ExchangeRateUpdater.Configuration;
using Serilog;

namespace ExchangeRateUpdater
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAppSettings(services);
            ConfigureCaching(services);
            ConfigureHttpClient(services);
            ConfigureLogging(services);
            services.AddSingleton<IExchangeRateService, ExchangeRateService>();
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.Configure<HttpServiceSettings>(Configuration.GetSection("HttpServiceSettings"));
            services.Configure<CzechApiSettings>(Configuration.GetSection("CzechApiSettings"));
        }

        private static void ConfigureCaching(IServiceCollection services)
        {
            services.AddMemoryCache();

            // Daily rates cache
            services.AddKeyedSingleton<ICnbRatesCache>(AppConstants.DailyRatesKeyedService, (provider, _) =>
                new CnbRatesCache(
                    provider.GetRequiredService<IMemoryCache>(),
                    AppConstants.DailyRatesCacheKey,
                    CzechTimeZoneExtensions.GetNextCzechBankUpdateUtc,
                    provider.GetRequiredService<ILogger<CnbRatesCache>>()));

            // Monthly rates cache
            services.AddKeyedSingleton<ICnbRatesCache>(AppConstants.MonthlyRatesKeyedService, (provider, _) =>
                new CnbRatesCache(
                    provider.GetRequiredService<IMemoryCache>(),
                    AppConstants.MonthlyRatesCacheKey,
                    () => CzechTimeZoneExtensions.GetNextMonthUpdateUtc(),
                    provider.GetRequiredService<ILogger<CnbRatesCache>>()));
        }

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient<ICzechApiClient, CzechApiClient>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler((provider, request) =>
                {
                    var settings = provider.GetRequiredService<IOptions<HttpServiceSettings>>().Value;

                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            retryCount: settings.RetryCount,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (outcome, delay, retry, ctx) =>
                            {
                                var logger = provider.GetRequiredService<ILogger<Startup>>();
                                logger.LogWarning(outcome.Exception, "Retry {Retry} after {Delay}s", retry, delay.TotalSeconds);
                            });
                })
                .AddPolicyHandler((provider, request) =>
                {
                    var settings = provider.GetRequiredService<IOptions<HttpServiceSettings>>().Value;
                    return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(settings.TimeoutSeconds));
                });
        }

        private static void ConfigureLogging(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });
        }
    }
}