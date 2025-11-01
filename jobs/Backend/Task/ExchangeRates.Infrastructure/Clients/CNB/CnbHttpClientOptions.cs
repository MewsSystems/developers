namespace ExchangeRates.Infrastructure.External.CNB
{
    public class CnbHttpClientOptions
    {
        public string BaseUrl { get; set; }
        public TimeOnly DailyRefreshTimeCZ { get; set; }

        public int TimeoutSeconds { get; set; } = 10;

        public int RetryCount { get; set; } = 3;
        public int RetryBaseDelaySeconds { get; set; } = 2;

        public int CircuitBreakerFailures { get; set; } = 2;
        public int CircuitBreakerDurationSeconds { get; set; } = 30;
    }
}
