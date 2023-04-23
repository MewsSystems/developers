using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Providers;

internal interface IExchangeRateProvider
{
    IAsyncEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    void PrintExchangeRates(IEnumerable<Currency> currencies);
}