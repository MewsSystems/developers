using System.Net;

namespace ExchangeRateUpdater.Api.Tests.Helpers;

public sealed class FakeHttpMessageHandler(HttpStatusCode statusCode, HttpContent? content = null) : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        var response = new HttpResponseMessage(statusCode) { Content = content };
        return Task.FromResult(response);
    }
}
