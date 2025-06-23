namespace ExchangeRateUpdater.Options
{
    public class HttpClientOptions
    {
        public int TimeoutSeconds { get; set; }
        public int RetryCount { get; set; }
        public int RetryWaitSeconds { get; set; }
    }
} 