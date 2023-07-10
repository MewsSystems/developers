using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Abstractions;
using ExchangeRateUpdater.CnbProvider.Abstractions;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbRateProvider _cnbRateProvider;

        public ExchangeRateProvider(ICnbRateProvider ratesProvider)
        {
            _cnbRateProvider = ratesProvider;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date)
        {
            if (currencies == null || !currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var currenciesSet = currencies.Select(s => s.Code.ToUpper()).ToHashSet();

            var rates = await _cnbRateProvider.GetRatesByDateAsync(date);

            return rates.Where(w => currenciesSet.Contains(w.SourceCurrency.Code.ToUpper()));
        }
    }
}
