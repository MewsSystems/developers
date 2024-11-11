namespace ExchangeRateUpdater.Domain.Config
{
    public class PollyConfig
    {
        public int RetryCountAttempts { get; set; }
        public int SleepRetrySeconds { get; set; }
    }
}
