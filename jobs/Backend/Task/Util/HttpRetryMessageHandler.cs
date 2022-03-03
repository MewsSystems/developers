using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Util;

public class HttpRetryMessageHandler : DelegatingHandler
{
    private const int RetryCount = 3;
    private const int BaseRetryWaitSec = 2;

    public HttpRetryMessageHandler(HttpClientHandler handler) : base(handler)
    {
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(BaseRetryWaitSec, retryAttempt)));
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken) => GetRetryPolicy().ExecuteAsync(() => base.SendAsync(request, cancellationToken));
}