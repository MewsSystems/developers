
using Polly.Retry;

namespace ExchangeRateUpdater.Features.Configuration
{
    public class ExchangeRateFeatureConfiguration
    {
        public string BaseUrl { get; set; }
        public RetryOptions RetryOptions { get; set; }
        public Func<AsyncRetryPolicy<HttpResponseMessage>> RetryHandler { get; set; }

    }

    public enum RetryOptions
    {
        Default,
        Custom
    }
}
