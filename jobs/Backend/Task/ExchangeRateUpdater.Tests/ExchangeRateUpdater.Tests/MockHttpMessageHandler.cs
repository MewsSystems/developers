namespace ExchangeRateUpdater.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    public HttpResponseMessage? ResponseMessage { get; set; }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(ResponseMessage!);
    }
}