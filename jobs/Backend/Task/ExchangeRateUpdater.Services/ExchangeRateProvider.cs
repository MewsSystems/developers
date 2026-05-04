using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;

namespace ExchangeRateUpdater.Services;

/// <summary>
/// Provides exchange rates using a specified client strategy.
/// </summary>
/// <param name="exchangeRatesClientStrategy"></param>
public class ExchangeRateProvider(IExchangeRatesClientStrategy exchangeRatesClientStrategy) : IExchangeRateProvider
{
    private readonly Currency referenceCurrency = new Currency("CZK");
    
    /// <summary>
    /// Gets exchange rates for the specified currencies.
    /// </summary>
    /// <param name="currencies"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var requested = currencies.ToHashSet();
        var cnbRates = await exchangeRatesClientStrategy.GetExchangeRates();

        var result = new List<ExchangeRate>();
        foreach (var rate in cnbRates)
        {
            var foreignCurrency = new Currency(rate.CurrencyCode);
            if (requested.Contains(foreignCurrency))
            {
                var valuePerUnit = rate.Rate / rate.Amount;
                result.Add(new ExchangeRate(foreignCurrency, this.referenceCurrency, valuePerUnit));
            }
        }

        return result;
    }
}
