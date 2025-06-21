namespace ExchangeRateUpdater.Infrastructure.Http
{
    public class CnbApiOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ExchangeRatesEndpoint { get; set; } = string.Empty;
        public int RequestTimeoutSeconds { get; set; } = 30;
        public int MaxRetries { get; set; } = 3;
        public double CircuitBreakerFailureThreshold { get; set; } = 0.5;
        public int CircuitBreakerSamplingDurationSeconds { get; set; } = 60;
        public int CircuitBreakerMinimumThroughput { get; set; } = 5;
        public int CircuitBreakerDurationOfBreakSeconds { get; set; } = 30;
    }
}