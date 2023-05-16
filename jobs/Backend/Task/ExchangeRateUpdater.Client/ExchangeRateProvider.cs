using ExchangeRateUpdater.Client.Client;
using ExchangeRateUpdater.Client.Contracts;

namespace ExchangeRateUpdater.Client;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private IProviderClient _client;
    public ExchangeRateProvider(IProviderClient client)
    {
        _client = client;
    }
    
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var data = _client.GetAsync().Result;
        if (!data.Any()) return new List<ExchangeRate>(0);
        var result = new List<ExchangeRate>();

        foreach (var currency in currencies)
        {
            var pair = data.FirstOrDefault(x => x.Code == currency.Code);
            if (pair == null) continue;
            var rate = new ExchangeRate(new Currency(pair.Code), new Currency("CZK"), pair.Rate);
            result.Add(rate);
        }
        
        return result;
    }
}