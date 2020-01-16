using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateLoader _exchangeRateLoader;

        public ExchangeRateProvider(IExchangeRateLoader exchangeRateLoader)
        {
            _exchangeRateLoader = exchangeRateLoader ?? throw new System.ArgumentNullException(nameof(exchangeRateLoader));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies is null) throw new System.ArgumentNullException(nameof(currencies));

            var distinctCurrencyCodes = currencies
                .Where(c => c != null)
                .Select(c => c.Code)
                .Distinct();

            var requestedCurrencyCodes = new HashSet<string>(distinctCurrencyCodes);

            var exchangeRates = _exchangeRateLoader.LoadExchangeRates()
                .GetAwaiter().GetResult();

            return exchangeRates.Where(e => requestedCurrencyCodes.Contains(e.TargetCurrency.Code));
        }
    }
}
