using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRatesFilter : IExchangeRatesFilter
    {
        public IEnumerable<ExchangeRate> GetFilteredRates(IEnumerable<ExchangeRate> unfilteredRates, 
            IEnumerable<Currency> currencies)
        {
            var rates = from currency in currencies
                        join unfilteredRate in unfilteredRates
                        on currency.Code equals unfilteredRate.SourceCurrency.Code
                        select unfilteredRate;

            return rates;
        }
    }
}
