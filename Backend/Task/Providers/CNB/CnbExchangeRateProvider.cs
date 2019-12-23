using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        /// <summary>
        /// URL for Exchange Rates of major currencies.
        /// </summary>
        private const string URL_MAJOR_CURRENCY_RATES = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        /// <summary>
        /// URL for Exchange Rates of minor currencies. These get updated less often than the major ones. 
        /// </summary>
        private const string URL_MINOR_CURRENCY_RATES = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.xml";

        private readonly ICnbClient _cnbClient;

        public CnbExchangeRateProvider(ICnbClient cnbClient)
        {
            _cnbClient = cnbClient;
        }

        public ProviderName ProviderName => ProviderName.CNB;

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRate>();

            if (currencies == null || !currencies.Any()) return result;

            /* making it run Sync here so that I won't have to change GetExchangeRates method return type to Task, 
             * because it might be currently in use in other parts of the application and it would require me to propagate async/await everywhere.
             */
            var majorResults = _cnbClient.ReadExchangeRatesFromUrlAsync(URL_MAJOR_CURRENCY_RATES).GetAwaiter().GetResult();

            var missingCurrencies = new List<Currency>();
            var czkCurrency = new Currency("CZK");

            foreach (var currency in currencies)
            {
                var majorCurrency = majorResults.FirstOrDefault(x => x.CurrencyCode == currency.Code);
                if(majorCurrency != null)
                {
                    result.Add(new ExchangeRate(new Currency(majorCurrency.CurrencyCode), czkCurrency, majorCurrency.ExchangeRateNormalized));
                }
                else
                {
                    missingCurrencies.Add(currency);
                }
            }

            //CNB devided their exchange rates into two groups so both of them have to be considered
            if (missingCurrencies.Any())
            {
                var minorResults = _cnbClient.ReadExchangeRatesFromUrlAsync(URL_MINOR_CURRENCY_RATES).GetAwaiter().GetResult();

                foreach (var currency in missingCurrencies)
                {
                    var minorCurrency = minorResults.FirstOrDefault(x => x.CurrencyCode == currency.Code);
                    if (minorCurrency != null)
                    {
                        result.Add(new ExchangeRate(new Currency(minorCurrency.CurrencyCode), czkCurrency, minorCurrency.ExchangeRateNormalized));
                    }
                }
            }

            return result;
        }
    }
}