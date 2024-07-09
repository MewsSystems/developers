using System.Collections.Generic;
using V1CzechNationBankExchangeRateProvider;

namespace ExchangeRateUpdater.Lib.Shared
{
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}