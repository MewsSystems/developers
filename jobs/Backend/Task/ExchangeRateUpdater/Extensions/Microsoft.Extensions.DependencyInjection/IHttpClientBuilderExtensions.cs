using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly.Timeout;
using Polly;

namespace ExchangeRateUpdater.Extensions.Microsoft.Extensions.DependencyInjection;
public static class IHttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddRetryWithExponentialWaitPolicy(this IHttpClientBuilder builder, int attempts, TimeSpan waitTime, TimeSpan attemptTimeout)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(attempts, attempt => TimeSpan.FromSeconds(Math.Pow(waitTime.TotalSeconds, attempt)));

        var singleTryTimeputPolicy = Policy.TimeoutAsync<HttpResponseMessage>(attemptTimeout);

        return builder
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(singleTryTimeputPolicy);
    }
}
