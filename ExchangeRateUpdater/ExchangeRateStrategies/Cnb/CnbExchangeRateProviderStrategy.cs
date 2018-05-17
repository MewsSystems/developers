using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Cnb
{
    public class CnbExchangeRateProviderStrategy : IExchangeRateProviderTargetCurrencyStrategy
    {
        public const string CnbRatesUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        private readonly ICnbRatesFetcher _fetcher;
        private readonly ICnbRatesParser _parser;

        public CnbExchangeRateProviderStrategy(ICnbRatesFetcher fetcher, ICnbRatesParser parser)
        {
            _fetcher = fetcher;
            _parser = parser;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(Currency targetCurrency, IEnumerable<Currency> currencies)
        {
            if (!targetCurrency.Equals(new Currency("CZK")))
            {
                throw new ArgumentException("CNB exchange rate provider requires CZK as target currency");
            }

            var contents = await _fetcher.FetchRatesAsync(CnbRatesUrl);

            var cnbRates = _parser
                .Parse(contents)
                .Distinct()
                .ToDictionary(rate => rate.CurrencyCode);

            return currencies
                .Where(currency => cnbRates.ContainsKey(currency.Code) && !currency.Equals(targetCurrency))
                .Select(currency => new ExchangeRate(
                    currency,
                    targetCurrency,
                    cnbRates[currency.Code].BaseRate));
        }
    }
}
