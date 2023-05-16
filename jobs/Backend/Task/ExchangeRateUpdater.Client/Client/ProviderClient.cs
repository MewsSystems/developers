using System.Text;
using ExchangeRateUpdater.Client.Exceptions;

namespace ExchangeRateUpdater.Client.Client;

public class ProviderClient : IProviderClient
{
    private readonly HttpClient _httpClient;

    public ProviderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<ExchangeRatePair>> GetAsync(DateTime? date = null)
    {
        var urlBuilder = new StringBuilder("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        if (date.HasValue)
        {
            urlBuilder.Append($"?date={date:dd.MM.yyyy}");
        }
        
        var response = await _httpClient.GetAsync(urlBuilder.ToString());
        if (!response.IsSuccessStatusCode)
        {
            throw new ExchangeRateProviderException(response.StatusCode, response.ReasonPhrase);
        }
        var responseString = await response.Content.ReadAsStringAsync();
        var reader = new StringReader(responseString);
        var result = new List<ExchangeRatePair>();
        byte skipHeaders = 2;
        while (await reader.ReadLineAsync() is { } line) {
            if (skipHeaders != 0)
            {
                skipHeaders--;
                continue;
            }
            var fields = line.Split('|');
            var country = fields[0];
            var currency = fields[1];
            var amount = decimal.Parse(fields[2]);
            var code = fields[3];
            var rate = decimal.Parse(fields[4]);
            var pair = new ExchangeRatePair(
                country,
                currency,
                amount,
                code,
                rate);
            result.Add(pair);
        }
        return result;
    }
}