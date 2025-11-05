using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace ExchangeRateUpdater.Src.Infrastructure;

public static class Policies
{
    public static IAsyncPolicy<HttpResponseMessage> CreateHttpPolicy(int retries, ILogger logger)
    {
        AsyncRetryPolicy<HttpResponseMessage> retry = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult(r => (int)r.StatusCode is >= 500 or 429 or 408)
            .WaitAndRetryAsync(
                Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(300), Math.Max(1, retries)),
                onRetry: (outcome, delay, attempt, context) =>
                {
                    var reason = outcome.Exception?.GetType().Name
                                 ?? $"{(int)outcome.Result!.StatusCode} {outcome.Result.ReasonPhrase}";
                    logger.LogWarning(
                        "HTTP retry {Attempt} in {Delay} due to {Reason}. Context: {Context}",
                        attempt, delay, reason, context);
                });

        AsyncCircuitBreakerPolicy<HttpResponseMessage> breaker = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult(r => (int)r.StatusCode is >= 500 or 429 or 408)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay, context) =>
                    logger.LogWarning("Circuit OPEN for {Delay}. Context: {Context}", breakDelay, context),
                onReset: (context) =>
                    logger.LogInformation("Circuit RESET. Context: {Context}", context),
                onHalfOpen: () =>
                    logger.LogInformation("Circuit HALF-OPEN"));

        return Policy.WrapAsync(breaker, retry);
    }
}
