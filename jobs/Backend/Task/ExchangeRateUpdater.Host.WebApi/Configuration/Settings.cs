using Serilog.Events;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    public class Settings : ISettings
    {
        public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Debug;
        public string CzechNationalBankBaseAddress { get; set; } = string.Empty;
        public bool EnableSwagger { get; set; } = true;
    }
}
