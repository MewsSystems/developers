using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Configuration;

internal sealed class PollyPolicies(
    ProviderOptions options,
    ILogger<PollyPolicies> logger)
{
    public IAsyncPolicy<HttpResponseMessage> RetryPolicy =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: options.RetryCount,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, _) =>
                {
                    logger.LogWarning(
                        "Retry {RetryCount} after {Delay}s due to {Reason}",
                        retryCount,
                        timespan.TotalSeconds,
                        outcome.Exception?.Message
                            ?? outcome.Result.StatusCode.ToString());
                });

    public IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: options.RetryCount,
                durationOfBreak: TimeSpan.FromSeconds(options.DurationOfCircuitBreakSeconds),
                onBreak: (_, duration) =>
                {
                    logger.LogError("Circuit breaker opened for {Duration}s", duration.TotalSeconds);
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker reset");
                });

    public IAsyncPolicy<HttpResponseMessage> TimeoutPolicy =>
        Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(options.TimeoutSeconds));
}