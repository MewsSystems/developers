using System.Collections.Generic;
using ExchangeRateUpdater.model;

namespace ExchangeRateUpdater.services;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}
