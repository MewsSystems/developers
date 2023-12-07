﻿using Serilog.Events;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    /// <inheritdoc/>
    internal class Settings : ISettings
    {
        /// <inheritdoc/>
        public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Debug;
        /// <inheritdoc/>
        public string CzechNationalBankBaseAddress { get; set; } = string.Empty;
        /// <inheritdoc/>
        public bool EnableSwagger { get; set; } = true;
        /// <inheritdoc/>
        public bool CacheEnabled { get; set; } = false;
        /// <inheritdoc/>
        public int CacheSize { get; set; } = 0;
        /// <inheritdoc/>
        public TimeSpan TodayDataCacheTtl { get; set; } = TimeSpan.Zero;
        /// <inheritdoc/>
        public TimeSpan OtherDatesCacheTtl { get; set; } = TimeSpan.Zero;
    }
}