using ExchangeRateUpdater.Cache;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider
    {
        private readonly TimeSpan _cacheLifetime = TimeSpan.FromMinutes(10);

        private readonly ICacheService<string, List<ExchangeRateRecord>> _cacheService;
        private readonly IExchangeService _exchangeService;

        public ExchangeRateProvider(ICacheService<string, List<ExchangeRateRecord>> cacheService, IExchangeService exchangeService)
        {
            _cacheService = cacheService;
            _exchangeService = exchangeService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, Currency targetCurrency)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException(nameof(currencies));
            }

            var cachedValue = _cacheService.Get(StringConstants.EXCHANGE_CACHE_KEY);
            IEnumerable<ExchangeRateRecord> rates = null;

            if (cachedValue == null)
            {
                rates = await _exchangeService.GetExchangeRatesAsync();

                _cacheService.Add(StringConstants.EXCHANGE_CACHE_KEY, rates.ToList(), _cacheLifetime);
            }
            else
            {
                rates = cachedValue;
            }

            var exchangeRates = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var rate = rates.FirstOrDefault(x => x.CurrencyCode == currency.Code);
                if (rate == null)
                    continue;
                exchangeRates.Add(new ExchangeRate(currency, targetCurrency, rate.Rate));
            }

            return exchangeRates;
        }
    }
}
