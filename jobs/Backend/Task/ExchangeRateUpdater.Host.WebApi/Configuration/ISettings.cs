using Serilog.Events;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    public interface ISettings
    {
        public bool EnableSwagger { get; }
        public LogEventLevel MinimumLogLevel { get; }
        public string CzechNationalBankBaseAddress { get; }
    }
}
