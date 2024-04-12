using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Repositories;

public class CzechExchangeRatesRepository : IExchangeRatesRepository
{

    private static readonly Currency CzechCurrency = new("CZK");
    
    public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
    {
        var ratesResponse = await CnbClient.GetAsync("/cnbapi/exrates/daily?lang=EN");
        ratesResponse.EnsureSuccessStatusCode();
        var czechRates = await ratesResponse.Content.ReadFromJsonAsync<DailyRatesResponse>();

        if (czechRates == null)
        {
            return [];
        }
        
        return czechRates.Rates.Select(rate => new ExchangeRate(
                new Currency(rate.CurrencyCode), 
                CzechCurrency, 
                rate.Rate / rate.Amount)) // dividing by amount here as the api can return a rate set for a different amount
            .ToList();
    }

    private static readonly HttpClient CnbClient = new()
    {
        BaseAddress = new Uri("https://api.cnb.cz"),
    };
}