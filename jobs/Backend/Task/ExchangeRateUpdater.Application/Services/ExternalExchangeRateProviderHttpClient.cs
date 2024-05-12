namespace ExchangeRateUpdater.Application.Services;

internal class ExternalExchangeRateProviderHttpClient
{
    public HttpClient Client { get; }

    public ExternalExchangeRateProviderHttpClient(HttpClient client)
    {
        Client = client;
    }
}
