namespace ExchangeRateUpdater;

public class ExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetExchangeRatesData(string url)
    {
        return await _httpClient.GetStringAsync(url);
    }
}
