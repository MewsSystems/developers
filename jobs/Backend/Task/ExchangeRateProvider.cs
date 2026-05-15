using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateSource _exchangeRateSource;

        public ExchangeRateProvider(IExchangeRateSource exchangeRateSource)
        {
            _exchangeRateSource = exchangeRateSource;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var cnbRates = GetSourceExchangeRates();
            var filteredRates = GetFilteredRates(cnbRates, currencies);
            return filteredRates;
        }

        private IEnumerable<ExchangeRate> GetSourceExchangeRates()
        {
            // Public API stays synchronous per assignment; the source uses async HTTP.
            var task = _exchangeRateSource.GetExchangeRates();
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private IEnumerable<ExchangeRate> GetFilteredRates(IEnumerable<ExchangeRate> cnbRates, IEnumerable<Currency> currencies)
        {
            return cnbRates.Where(rate => currencies.Contains(rate.SourceCurrency));
        }
    }
}


