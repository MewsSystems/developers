using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly string _url;

    public ExchangeRateProvider(string dailyExchangeRatesUrl)
    {
        _url = dailyExchangeRatesUrl;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        DailyExchangeRateResponse? rates = await GetDailyExchangeRates();
        if (rates?.Rates is null)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        return rates.Rates
            .Where(r => !string.IsNullOrWhiteSpace(r.CurrencyCode))
            .Where(r => currencies.Contains(new Currency(r.CurrencyCode!)))
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
            
        try
        {
            using var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_url)
            };

            var response = await client.SendAsync(request);
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