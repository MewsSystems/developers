using System.Text.Json;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRatesConfig _exchangeRatesConfig;

    public ExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeRatesConfig> exchangeRatesConfig)
    {
        _httpClient = httpClient;
        _exchangeRatesConfig = exchangeRatesConfig.Value;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        if (_exchangeRatesConfig.Currencies is null)
        {
            throw new Exception("Exchange Rates Currencies is null");
        }
        
        DailyExchangeRateResponse? rates = await GetDailyExchangeRates();
        if (rates?.Rates is null)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        return rates.Rates
            .Where(r => !string.IsNullOrWhiteSpace(r.CurrencyCode))
            .Where(r => _exchangeRatesConfig.Currencies.Contains(new Currency(r.CurrencyCode!)))
            .Select(r =>
            {
                var source = new Currency(r.CurrencyCode!);
                
                // Target seems to always be CZK, because the data is from the Czech Bank's point of view.
                var target = new Currency("CZK");
                var rate = r.Rate / r.Amount;
                    
                return new ExchangeRate(source, target, rate);
            });
    }
    
    private async Task<DailyExchangeRateResponse?> GetDailyExchangeRates()
    {
        DailyExchangeRateResponse? result = null;

        if (_exchangeRatesConfig.Url is null)
        {
            throw new Exception("Exchange Rates URL is null");
        }
        
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_exchangeRatesConfig.Url)
            };

            using var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<DailyExchangeRateResponse>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            }
            else
            {
                Console.WriteLine($"Failed to retrieve daily exchange rates: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while retrieving daily exchange rates: {e}");
        }

        return result;
    }
}