using ExchangeRateUpdater.Data;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRateProvider
{
    IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}