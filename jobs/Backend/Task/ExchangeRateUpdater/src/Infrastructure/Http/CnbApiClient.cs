using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace ExchangeRateUpdater.Infrastructure.Http
{
    /// <summary>
    /// Implementation of <see cref="ICnbApiClient"/> that retrieves exchange rates from the Czech National Bank (CNB) API.
    /// Uses Polly for resilience with retry, circuit breaker, timeout, and fallback policies.
    /// The client is configured via CnbApiOptions, which can be set in appsettings.json.
    /// </summary>
    public class CnbApiClient : ICnbApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CnbApiClient> _logger;
        private readonly CnbApiOptions _options;
        private readonly IAsyncPolicy<string> _policy;

        public CnbApiClient(HttpClient httpClient, ILogger<CnbApiClient> logger, IOptions<CnbApiOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.RequestTimeoutSeconds);

            _policy = CreatePolicies();
        }
        /// <summary>
        /// Retrieves latest exchange rates from the Czech National Bank (CNB) API.
        /// </summary>
        /// <returns>The latest exchange rates as a string.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<string> GetLatestExchangeRatesAsync()
        {
            try
            {
                return await _policy.ExecuteAsync(async () =>
                {
                    _logger.LogDebug("Attempting to retrieve exchange rates from CNB API.");
                    using var response = await _httpClient.GetAsync(_options.ExchangeRatesEndpoint);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogDebug("Successfully retrieved exchange rates from CNB API.");
                    return content;
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to retrieve exchange rates from CNB API.");
                throw new HttpRequestException("Failed to retrieve exchange rates from CNB API.", e);
            }
        }

        private IAsyncPolicy<string> CreatePolicies()
        {
            var retryPolicy = Policy<string>
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .Or<BrokenCircuitException>()
                .WaitAndRetryAsync(_options.MaxRetries, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, delay, retryCount, _) =>
                {
                    var ex = exception.Exception ?? new Exception(exception.ToString() ?? "Unknown error");
                    _logger.LogWarning(ex, "Retry {RetryCount} for request due to: {ExceptionMessage}. Waiting {Delay} before next attempt.",
                        retryCount, ex.Message, delay);
                });

            var circuitBreakerPolicy = Policy<string>
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .AdvancedCircuitBreakerAsync(
                    failureThreshold: _options.CircuitBreakerFailureThreshold,
                    samplingDuration: TimeSpan.FromSeconds(_options.CircuitBreakerSamplingDurationSeconds),
                    minimumThroughput: _options.CircuitBreakerMinimumThroughput,
                    durationOfBreak: TimeSpan.FromSeconds(_options.CircuitBreakerDurationOfBreakSeconds),
                    onBreak: (exception, breakDelay) =>
                    {
                        var ex = exception.Exception ?? new Exception(exception.ToString() ?? "Unknown error");
                        _logger.LogWarning(
                            ex,
                            "Circuit breaker opened for {BreakDelay} seconds",
                            breakDelay.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit breaker reset, flow is now normal");
                    },
                    onHalfOpen: () =>
                    {
                        _logger.LogInformation("Circuit breaker half-open, testing if service has recovered");
                    });

            var timeoutPolicy = Policy.TimeoutAsync<string>(
                TimeSpan.FromSeconds(_options.RequestTimeoutSeconds),
                TimeoutStrategy.Optimistic);

            var fallbackPolicy = Policy<string>
                .Handle<BrokenCircuitException>()
                .Or<TimeoutRejectedException>()
                .Or<HttpRequestException>()
                .FallbackAsync(
                    fallbackValue: string.Empty,
                    onFallbackAsync: async (outcome) =>
                    {
                        var ex = outcome.Exception ?? new Exception(outcome.ToString() ?? "Unknown error");
                        _logger.LogWarning(ex, "Fallback triggered due to error");
                        await Task.CompletedTask;
                    });

            return Policy.WrapAsync(fallbackPolicy, Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy));
        }
    }
}
