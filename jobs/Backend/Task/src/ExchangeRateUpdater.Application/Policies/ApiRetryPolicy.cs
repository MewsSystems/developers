using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application
{
    public class ApiRetryPolicy
    {
        private readonly ILogger<ApiRetryPolicy> _logger;

        public ApiRetryPolicy(ILogger<ApiRetryPolicy> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .Or<Exception>()
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        _logger.LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                    });
        }
    }
}
