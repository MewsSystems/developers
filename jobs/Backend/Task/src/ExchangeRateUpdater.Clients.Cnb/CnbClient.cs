namespace ExchangeRateUpdater.Clients.Cnb;

public class CnbClient : ICnbClient
{
    private HttpClient _httpClient;

    public CnbClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}