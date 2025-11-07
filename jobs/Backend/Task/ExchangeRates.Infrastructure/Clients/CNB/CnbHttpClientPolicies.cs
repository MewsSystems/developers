using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRates.Infrastructure.Clients.CNB
{
    public static class CnbHttpClientPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy(CnbHttpClientOptions options, ILogger logger)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    options.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(options.RetryBaseDelaySeconds, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        logger.LogWarning(
                            "Retry {RetryAttempt} after {Delay}s due to {Reason}",
                            retryAttempt,
                            timespan.TotalSeconds,
                            outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()
                        );
                    });
        }

        public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy(CnbHttpClientOptions options)
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(
                TimeSpan.FromSeconds(options.TimeoutSeconds)
            );
        }
    }
}
