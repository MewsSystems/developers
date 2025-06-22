namespace ExchangeRateUpdater.Infrastructure.Http
{
    /// <summary>
    /// Provides configuration options for the CnbApiClient.
    /// These options are typically configured in appsettings.json and bound at runtime.
    /// </summary>
    public class CnbApiOptions
    {
        /// <summary>
        /// Gets or sets the base URL for the Czech National Bank (CNB) API.
        /// </summary>
        public string BaseUrl { get; set; } = "https://www.cnb.cz/";
        
        /// <summary>
        /// Gets or sets the endpoint for retrieving daily exchange rates.
        /// </summary>
        public string ExchangeRatesEndpoint { get; set; } = "en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
        
        /// <summary>
        /// Gets or sets the timeout in seconds for a single HTTP request to the CNB API.
        /// </summary>
        public int RequestTimeoutSeconds { get; set; } = 30;
        
        /// <summary>
        /// Gets or sets the maximum number of retries for a failed request.
        /// </summary>
        public int MaxRetries { get; set; } = 3;
        
        /// <summary>
        /// Gets or sets the failure threshold for the circuit breaker (e.g., 0.5 means 50% of requests failed).
        /// </summary>
        public double CircuitBreakerFailureThreshold { get; set; } = 0.5;
        
        /// <summary>
        /// Gets or sets the duration in seconds over which failures are measured for the circuit breaker.
        /// </summary>
        public int CircuitBreakerSamplingDurationSeconds { get; set; } = 60;
        
        /// <summary>
        /// Gets or sets the minimum number of requests in the sampling duration before the circuit breaker can open.
        /// </summary>
        public int CircuitBreakerMinimumThroughput { get; set; } = 5;
        
        /// <summary>
        /// Gets or sets the duration in seconds for which the circuit breaker will remain open before transitioning to half-open.
        /// </summary>
        public int CircuitBreakerDurationOfBreakSeconds { get; set; } = 30;
    }
}