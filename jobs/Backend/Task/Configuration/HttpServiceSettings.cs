namespace ExchangeRateUpdater.Configuration
{
    public class HttpServiceSettings
    {
        public int RetryCount { get; set; } = 3;
        public int TimeoutSeconds { get; set; } = 30;
    }
}