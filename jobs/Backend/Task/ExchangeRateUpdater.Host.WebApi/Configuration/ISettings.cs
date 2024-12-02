using Serilog.Events;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    /// <summary>
    /// Settings interface to configure the host/adapters.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Enables Swagger.
        /// </summary>
        public bool EnableSwagger { get; }
        /// <summary>
        /// Enables in-memory cache.
        /// </summary>
        public bool CacheEnabled { get; }
        /// <summary>
        /// Sets the Cache Size until cache eviction is performed.
        /// </summary>
        public int CacheSize { get; }
        /// <summary>
        /// Sets MinimumLogLevel.
        /// </summary>
        public LogEventLevel MinimumLogLevel { get; }
        /// <summary>
        /// Provides the base address for the CzechNationalBank.
        /// </summary>
        public string CzechNationalBankBaseAddress { get; }
        /// <summary>
        /// TTL for today's data.
        /// </summary>
        public TimeSpan TodayDataCacheTtl { get; }
        /// <summary>
        /// TTL for other dates data except today.
        /// </summary>
        public TimeSpan OtherDatesCacheTtl { get; }
    }
}
