using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbClient _cnbClient;
        private readonly CnbMapper _cnbMapper;
        private readonly IExchangeRateCache _exchangeRateCache;

        public ExchangeRateProvider(ICnbClient cnbClient, IExchangeRateCache exchangeRateCache)
        {
            _cnbClient = cnbClient;
            this._exchangeRateCache = exchangeRateCache;

            _cnbMapper = new CnbMapper();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            ExchangeRate[] exchangeRates = null;
            if (!_exchangeRateCache.TryGetValue(out exchangeRates))
            {
                exchangeRates = await FetchExchangeRates();
                _exchangeRateCache.Set(exchangeRates);
            }

            return exchangeRates.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));
        }

        private async Task<ExchangeRate[]> FetchExchangeRates()
        {
            var dailyRates = await _cnbClient.GetLatestExchangeRatesAsync();

            CountExchangeRatesAgeMetric(dailyRates.Date);

            return dailyRates.Rates
                .Select(_cnbMapper.MapExchangeRate)
                .ToArray();
        }

        private void CountExchangeRatesAgeMetric(DateOnly exchangeRateDate)
        {
            // Here we can publish the time difference between current date and exchangeRateDate to our metrics store.
            // Alerting stale exchange rates is not trivial because they are not published on weekends and bank holidays.
            // If business is not particularly sensitive to it, a simple rule of "rates must be at most 7 days old" would
            // suffice. Otherwise, tracking bank holidays (https://www.cnb.cz/en/public/media-service/schedules-and-other-info/bank-holidays-in-the-czech-republic)
            // should be implemented and the staleness counted since the last working day.
        }
    }
}
