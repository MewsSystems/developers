namespace Mews.BackendDeveloperTask.ExchangeRates.Cnb;

public class CnbTextExchangeRateRetriever : ICnbTextExchangeRateRetriever
{
    private readonly HttpClient _client;

    public CnbTextExchangeRateRetriever(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<string> GetDailyRatesAsync()
    {
        var dailyRates = await _client.GetAsync("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        var content = await dailyRates.Content.ReadAsStringAsync();
        return content;
    }
}
