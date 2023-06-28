using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater;

public interface IExchangeRateProvider
{
    IReadOnlyCollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}