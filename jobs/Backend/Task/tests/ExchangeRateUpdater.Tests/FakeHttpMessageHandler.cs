
namespace ExchangeRateUpdater.Tests;

public class FakeHttpMessageHandler(HttpResponseMessage responseMessage) : DelegatingHandler
{
    private readonly HttpResponseMessage _fakeResponse = responseMessage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_fakeResponse);
    }
}

public class FakeHttpMessageHandlerWithException : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("Unexpected error");
    }
}
