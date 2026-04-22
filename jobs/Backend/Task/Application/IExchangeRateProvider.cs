using System.Collections.Generic;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Returns the latest available rates for the requested source currencies against CZK.
    /// This contract is intentionally synchronous (`sync-first`) in this iteration.
    /// </summary>
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}
