using System.Collections.Generic;
using ExchangeRateUpdater.Dtos;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}