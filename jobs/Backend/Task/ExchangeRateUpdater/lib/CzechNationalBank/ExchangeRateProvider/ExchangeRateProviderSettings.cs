using Microsoft.Extensions.Configuration;
using System.IO;
using ExchangeRateUpdater.Lib.Shared;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    public class ExchangeRateProviderSettings : IExchangeRateProviderSettings
    {
        public string SourceUrl { get; internal set; }
        public int TimeoutSeconds { get; internal set; }
        public int MaxThreads { get; internal set; }
        public int RateLimitCount { get; internal set; }
        public int RateLimitDuration { get; internal set; }
        public int Precision { get; internal set; }
        public bool PipeOutput { get; internal set; }

        public ExchangeRateProviderSettings(
            string sourceUrl,
            int timeoutSeconds,
            int maxThreads,
            int rateLimitCount,
            int rateLimitDuration,
            int precision
            )
        {
            SourceUrl = sourceUrl;
            TimeoutSeconds = timeoutSeconds;
            MaxThreads = maxThreads;
            RateLimitCount = rateLimitCount;
            RateLimitDuration = rateLimitDuration;
            Precision = precision;
        }

        /// <summary>
        /// Utility function to Load settings from the appsettings.json
        /// </summary>
        /// <returns>
        /// ExchangeRateProviderSettings
        /// </returns>
        public static ExchangeRateProviderSettings LoadFromAppSettings()
        {

            // ideally we would get settings using dependency injection
            var path = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
              .SetBasePath(path)
              .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string sourceUrl = configuration["source_url"];

            int.TryParse(configuration["request_timeout_seconds"], out int timeoutSeconds);
            int.TryParse(configuration["max_thread_count"], out int maxThreads);
            int.TryParse(configuration["rate_limit_count"], out int rateLimitCount);
            int.TryParse(configuration["rate_limit_duration_seconds"], out int rateLimitDurationSeconds);
            int.TryParse(configuration["precision_level"], out int precisionLevel);

            return new ExchangeRateProviderSettings(
                sourceUrl: sourceUrl,
                timeoutSeconds: timeoutSeconds,
                maxThreads: maxThreads,
                rateLimitCount: rateLimitCount,
                rateLimitDuration: rateLimitDurationSeconds,
                precision: precisionLevel
            );
        }
    }

}