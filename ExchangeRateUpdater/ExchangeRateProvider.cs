using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRatesParser _ratesParser;
        private readonly IExchangeRatesConnector _ratesConnector;
        private IDictionary<string, decimal> _ratesToCZKList = new Dictionary<string, decimal>();


        public ExchangeRateProvider(IExchangeRatesConnector ratesConnector, IExchangeRatesParser ratesParser)
        {
            _ratesParser = ratesParser ?? throw new ArgumentNullException(nameof(ratesParser));
            _ratesConnector = ratesConnector ?? throw new ArgumentNullException(nameof(ratesConnector));
            GetDayRates();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();

            foreach (Currency fromCurrency in currencies)
            {
                if (!_ratesToCZKList.ContainsKey(fromCurrency.Code))
                {
                    continue;
                }

                foreach (Currency toCurrency in currencies)
                {
                    if (fromCurrency.Code == toCurrency.Code ||
                        !_ratesToCZKList.ContainsKey(toCurrency.Code))
                    {
                        continue; ;
                    }

                    ExchangeRate rate = GetCurrencyPairRate(fromCurrency, toCurrency);
                    result.Add(rate);
                }
            }

            return result;
        }

        private ExchangeRate GetCurrencyPairRate(Currency fromCurrency, Currency toCurrency)
        {
            decimal fromCurrencyToCZKRate = _ratesToCZKList[fromCurrency.Code];
            decimal toCurrencyToCZKRate = _ratesToCZKList[toCurrency.Code];

            decimal pairRate = fromCurrencyToCZKRate / toCurrencyToCZKRate;

            return new ExchangeRate(fromCurrency, toCurrency, pairRate);
        }

        private void GetDayRates()
        {
            _ratesToCZKList = new Dictionary<string, decimal>() {{"CZK", 1}};
            string ratesData = _ratesConnector.GetDayRatesInRawFormat();
            IDictionary<string, decimal> parsedRateList = _ratesParser.ParseRatesData(ratesData);

            _ratesToCZKList = _ratesToCZKList.Concat(parsedRateList).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
