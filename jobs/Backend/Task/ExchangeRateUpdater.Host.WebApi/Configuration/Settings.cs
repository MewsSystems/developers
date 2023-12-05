using Serilog.Events;

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
    }
}
