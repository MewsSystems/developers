using System;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Extensions.Http;
using Polly;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.HttpClients;

namespace ExchangeRateUpdater.Configuration
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
            // Bind configurations
            services.Configure<CzechBankSettings>(Configuration.GetSection("CzechBankSettings"));
            services.AddSingleton<ExchangeRateProvider>();


            // Add dependencies
            services.AddMemoryCache();

            // Register the parser
            services.AddSingleton<IExchangeRateParser>(sp => new CzechNationalBankTextRateParser(5,
            sp.GetRequiredService<ILogger<CzechNationalBankTextRateParser>>()));
            
            // === Add resilient HTTP clients with Polly ===
            services.AddHttpClient<DailyExchangeRateFetcher>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;
                    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                })
                .AddPolicyHandler((provider, request) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;

                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            retryCount: settings.RetryCount,
                            sleepDurationProvider: retryAttempt =>
                                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        );
                });

            services.AddHttpClient<OtherCurrencyExchangeRateFetcher>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;
                    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                })
                .AddPolicyHandler((provider, request) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;

                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            retryCount: settings.RetryCount,
                            sleepDurationProvider: retryAttempt =>
                                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        );
                });

            // Register fetchers
            services.AddSingleton<IExhangeRateFetcher, DailyExchangeRateFetcher>();
            services.AddSingleton<IExhangeRateFetcher, OtherCurrencyExchangeRateFetcher>();

            // Register main provider
            services.AddSingleton<IExchangeRateProviderService, ExchangeRateProviderService>();

            // Add logging
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddConsole();
            });
        }
    }
}
