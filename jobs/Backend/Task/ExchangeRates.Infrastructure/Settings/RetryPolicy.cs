namespace ExchangeRates.Infrastructure.Settings
{
    internal class RetryPolicy
    {
        public int RetryCount { get; set; }

        public int RetryAttemptInSeconds { get; set; }
    }
}
