namespace ExchangeRateUpdater.Domain.Config
{
    public class PollyConfig
    {
        public int RetryCountAttempts { get; set; }
        public int SleepRetrySeconds { get; set; }
        public int DurationOfBreakSeconds { get; set; }
        public int ExceptionsAllowedBeforeBreaking { get; set; }
    }
}
