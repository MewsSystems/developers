namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;

internal class TestHttpClientFactory : IHttpClientFactory, IDisposable
{
    private readonly Uri _baseUrl;
    private readonly HttpClient _httpClient;

    public TestHttpClientFactory(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or whitespace.", nameof(baseUrl));
        }

        _baseUrl = new Uri(baseUrl);
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = _baseUrl;
        _httpClient.Timeout = Timeout.InfiniteTimeSpan;
    }
    public HttpClient CreateClient(string name)
    {
        return _httpClient;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
