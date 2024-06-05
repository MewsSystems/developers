using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IEnumerable<ExchangeRate> _exchangeRates;

        public ExchangeRateProvider(IEnumerable<ExchangeRate> exchangeRates)
        {
            _exchangeRates = exchangeRates;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            if (_exchangeRates != null)
            {
                foreach (Currency currentCurrency in currencies)
                {
                    ExchangeRate found = _exchangeRates.SingleOrDefault(rate => rate.SourceCurrency.Code == currentCurrency.Code);
                    if (found != null)
                    {
                        exchangeRates.Add(found);
                    }
                }
            }
            return exchangeRates;
        }
    }
}
