using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.App;

public class ExchangeRateProvider
{
    private readonly IExchangeRateClient _client;

    public ExchangeRateProvider(IExchangeRateClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var response = await _client.GetExchangeRateAsync(DateTime.Now);
        if (string.IsNullOrEmpty(response))
            throw new Exception("Exchange rate server returned empty response");

        var rates = ExchangeRateParser.ParseExchangeRates(response);

        return rates.Where(rate => currencies.Contains(rate.TargetCurrency));
    }
}