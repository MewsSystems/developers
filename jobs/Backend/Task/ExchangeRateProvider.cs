using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;

namespace ExchangeRateUpdater
{
    internal sealed class ExchangeRateProvider
    {
        private readonly IBankApi _bankApi;

        public ExchangeRateProvider(IBankApi bankApi)
        {
            _bankApi = bankApi;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency>? currencies)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException(nameof(currencies), "currencies is null");
            }
            var cnbRates = await _bankApi.GetTodayExchangeRates();
            var selectedRates = cnbRates.Select(x => ExchangeRate.FromCZK(new Currency(x.CurrencyCode), x.RealRate))
                // TODO: seems should be filtered by source currency as well, but our source is always CZK I only need target currency then?
                .Where(x => currencies.Contains(x.TargetCurrency));
            return selectedRates;
        }
    }
}
