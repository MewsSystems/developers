namespace ExchangeRateUpdater.Tests.CnbClient.Integration;

internal class TestHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name = null!) => new();
}