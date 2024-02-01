namespace ExchangeRateUpdater.Providers.Tests;

public class TestHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage _response;

    public TestHttpMessageHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}
