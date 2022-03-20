using System.Collections.Generic;

namespace ExchangeRateUpdater.Parse
{
    public interface IExchangeRatesParser
    {
        IEnumerable<ExchangeRate> ParseRates(string exchangeRatesTxt);
    }
}
