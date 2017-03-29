using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IRate
    {
        string GetCurrencyCode();
        Dictionary<string, decimal> GetRates();
    }
}