using System.Net;

namespace ExchangeRateUpdater.Test;

public class MockHttpMessageHandler(HttpStatusCode httpStatusCode, HttpContent? httpContent = null) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage(httpStatusCode)
        {
            RequestMessage = request,
            Content = httpContent
        });
    }
}