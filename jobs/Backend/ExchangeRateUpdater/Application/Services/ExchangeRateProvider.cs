using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateProvider 
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class ExchangeRateProvider
    {
        private readonly Currency _baseCurrency;
        private readonly decimal _roundingDecimal;

        public ExchangeRateProvider(Currency baseCurrency, decimal roundingDecimal)
        {
            _baseCurrency = baseCurrency;
            _roundingDecimal = roundingDecimal;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();

            foreach (Currency c in currencies) 
            {
                var exchangeRate = new ExchangeRate(_baseCurrency, c, _roundingDecimal);

                // do some comparison if ex rate exists in list or not

                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }
    }
}
