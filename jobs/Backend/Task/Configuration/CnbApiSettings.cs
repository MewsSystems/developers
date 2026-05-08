namespace ExchangeRateUpdater.Configuration
{
    // Configuration settings for CNB API.
    public record CnbApiSettings
    {
        // Base URL for the CNB exchange rate API endpoint.
        public string BaseUrl { get; init; } = null!;

        // HTTP request timeout in seconds.
        public int TimeoutSeconds { get; init; }

        // Number of retry attempts for transient failures.
        public int RetryCount { get; init; }

        // Delay between retry attempts in seconds.
        public int RetryDelaySeconds { get; init; }
    }
}
