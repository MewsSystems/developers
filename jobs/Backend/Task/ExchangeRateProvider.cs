using System.Collections.Generic;
using ExchangeRateUpdater.Chain_of_Responsibility;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private Handler handler;
        private List<ExchangeRate> _exchangeRates;

        public ExchangeRateProvider()
        {
            _exchangeRates = new();

            handler = new Redis();
            Chain_of_Responsibility.CNB cnb = new();
            handler.SetNext(cnb);
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            foreach (Currency currency in currencies)
            {
                ExchangeRate exchangeRate = handler.GetExchangeRate(currency);

                if(exchangeRate is not null)
                { 
                    _exchangeRates.Add(exchangeRate);
                }
            }

            return _exchangeRates;
        }
    }
}
