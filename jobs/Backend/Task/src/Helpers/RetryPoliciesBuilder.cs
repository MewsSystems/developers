using Polly;
using RestSharp;
using System;

namespace ExchangeRateUpdater.Helpers
{
    public interface IRetryPoliciesBuilder
    {
        IAsyncPolicy<RestResponse> BuildExponentialBackoff();
    }

    public class RetryPoliciesBuilder : IRetryPoliciesBuilder
    {
        private const int MAX_RETRIES = 3;

        public IAsyncPolicy<RestResponse> BuildExponentialBackoff()
        {
            return Policy.HandleResult<RestResponse>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(MAX_RETRIES, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
