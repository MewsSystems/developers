using ExchangeRateUpdater.Helpers;
using Polly;
using RestSharp;
using System;

namespace ExchangeRateUpdater.UnitTests.Helpers
{
    public class FakeRetryPoliciesBuilder : IRetryPoliciesBuilder
    {
        public IAsyncPolicy<RestResponse> BuildExponentialBackoff()
        {
            return Policy.HandleResult<RestResponse>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, x => TimeSpan.FromMilliseconds(50));
        }
    }
}
