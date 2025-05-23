using System.Net.Http;
using System;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Extensions.Http;
using Polly;

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
            services.Configure<HttpServiceSettings>(Configuration.GetSection("HttpServiceSettings"));


            // Add dependencies
            services.AddMemoryCache();
            services.AddHttpClient<IExchangeRateProviderService, ExchangeRateProviderService>()
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
