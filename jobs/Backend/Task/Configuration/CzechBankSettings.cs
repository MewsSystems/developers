namespace ExchangeRateUpdater.Configuration
{
    public class CzechBankSettings
    {
        public string DailyRatesUrl { get; set; }
        public string OtherCurrencyRatesUrl { get; set; }
        public int TimeoutSeconds { get; set; } = 10;
        public int RetryCount { get; set; } = 3;
    }
}
