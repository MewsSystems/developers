using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using CnbApi.Client;

namespace ExchangeRateUpdater;
public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
}
public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICnbClient _cnbClient;
    public ExchangeRateProvider(ICnbClient cnbClient)
    {
        _cnbClient = cnbClient;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        // Arguably a null list is an exception to be logged, but context is everything. In this instance we'll just handle it gracefully. 
        if (currencies == null)
            return Enumerable.Empty<ExchangeRate>();

        var bankRates = await _cnbClient.GetDailyExchangeRates(DateTime.Now);
        if(bankRates == null)
        {
            throw new InvalidOperationException("Failed to obtain bank rates from CNB Api");
        }

        var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code));
        var exchangeRateList = bankRates.Rates
            .Where(r => currencyCodes.Contains(r.CurrencyCode))
            .Select(r => r.ToExchangeRate())
            .ToList();

        return exchangeRateList;
    }

}

