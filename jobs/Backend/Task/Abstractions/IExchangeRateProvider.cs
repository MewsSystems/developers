using System.Collections.Generic;
using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRateProvider
{
    IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}