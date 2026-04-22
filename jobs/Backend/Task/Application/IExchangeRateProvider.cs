using System.Collections.Generic;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}
