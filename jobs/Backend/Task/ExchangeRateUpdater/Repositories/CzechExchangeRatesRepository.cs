using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Repositories;

public class CzechExchangeRatesRepository : IExchangeRatesRepository
{
    private readonly HttpClient _cnbHttpClient;

    private static readonly Currency CzechCurrency = new("CZK");

    public CzechExchangeRatesRepository(HttpClient cnbHttpClient)
    {
        _cnbHttpClient = cnbHttpClient;
    }

    public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
    {
        var ratesResponse = await _cnbHttpClient.GetAsync("/cnbapi/exrates/daily?lang=EN");
        ratesResponse.EnsureSuccessStatusCode();
        var czechRates = await ratesResponse.Content.ReadFromJsonAsync<DailyRatesResponse>();

        if (czechRates == null)
        {
            return [];
        }
        
        return czechRates.Rates.Select(rate => new ExchangeRate(
                new Currency(rate.CurrencyCode), 
                CzechCurrency, 
                NormaliseRate(rate.Rate, rate.Amount))) // dividing by amount here as the api can return a rate set for a different amount
            .ToList();
    }

    private static decimal NormaliseRate(decimal rate, int amount)
    {
        return amount > 0 ? rate / amount : rate;
    }
}
