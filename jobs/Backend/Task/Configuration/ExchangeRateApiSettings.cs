namespace ExchangeRateUpdater.Configuration
{
    /// <summary>
    /// Configuration settings for exchange rate API access - Generic structure usable for any exchange rate source.
    /// Compile time binding via IOptions pattern better than raw string keys
    /// </summary>
    public class ExchangeRateApiSettings
    {
        public string DailyRatesUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
    }
}