using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Util;

public static class PollyRetryHelper
{
    private const int RetryCount = 3;
    private const int BaseRetryWaitSec = 2;

    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(BaseRetryWaitSec, retryAttempt)));
    }
}