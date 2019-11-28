using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbProvider;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private IReadOnlyDictionary<Currency, ExchangeRate> _rates = new Dictionary<Currency, ExchangeRate>();

        private readonly ICustomExchangeRatesProvider _provider;

        public ExchangeRateProvider(ICustomExchangeRatesProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public ExchangeRateProvider():this(new CnbExchangeRatesProvider(new CnbFxRatesWebLoader(new CnbFxRateRowParser())))
        {
            //Actual provider should be injected of course but adding container here would be an overkill
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            SyncRates();
            foreach (var currency in currencies)
            {
                if (_rates.TryGetValue(currency, out var returnValue))
                   yield return returnValue;
            }
        }

        private void SyncRates()
        {
            var result = Task.Run(_provider.GetAllRatesAsync).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.ratesWhereUpdated && result.rates != null)
            {
                _rates = result.rates.ToDictionary(x => x.TargetCurrency, x => x);
            }
        }
    }
}
