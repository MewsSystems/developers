using Microsoft.Extensions.Logging;

namespace ExchangeRateProviderCzechNationalBank.Logging
{
    public static partial class LoggerMessageDefinitions
    {
        [LoggerMessage(EventId = -1, Level = LogLevel.Warning, Message = "Unable to retrieve ExchangeRates from Czech National Bank. Data source: {CNBUri}")]
        public static partial void LogCzechNationalBankResponseFailure(this ILogger logger, string CNBUri, Exception ex);
    }
}
