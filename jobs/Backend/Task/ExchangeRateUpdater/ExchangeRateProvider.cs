using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ILogger<ExchangeRateProvider> logger;
        private readonly IExchangeRateLoader loader;
        private readonly IMemoryCache currencyCache;
        private readonly IDateTimeService dateTimeService;

        public ExchangeRateProvider(IExchangeRateLoader loader,
                                    ILogger<ExchangeRateProvider> logger,
                                    IMemoryCache currencyCache,
                                    IDateTimeService dateTimeService)
        {
            this.loader = loader;
            this.logger = logger;
            this.currencyCache = currencyCache;
            this.dateTimeService = dateTimeService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 
        // discussable: do we need force manual cleaning cache during runtime?
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            ArgumentNullException.ThrowIfNull(currencies);

            if (!currencies.Any()) return [];

            var ratesFromCache = GetRatesFromCache(currencies);
            var currenciesFromCache = ratesFromCache.Select(r => r.SourceCurrency);
            var currenciesToLoad = currencies.Except(currenciesFromCache).ToList();
            if (currenciesToLoad.Count == 0)
            {
                logger.LogInformation("All rates loaded from cache - no need to call Loader");
                return ratesFromCache;
            }

            var ratesFromDataSource = await GetRatesFromDataSource(currenciesToLoad);

            PutRatesToCache(ratesFromDataSource);

            logger.LogInformation("Loaded {} rates from data source, {} rates from cache", ratesFromDataSource.Count, ratesFromCache.Count);

            return ratesFromDataSource.Union(ratesFromCache);
        }

        private List<ExchangeRate> GetRatesFromCache(IEnumerable<Currency> currencies)
        {
            var ratesFromCache = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                if (currencyCache.TryGetValue<ExchangeRate>(currency.Code, out var exchangeRate))
                {
                    ratesFromCache.Add(exchangeRate);
                }
            }

            return ratesFromCache;
        }

        // discussable: can handle exceptions and return empty result, or can re-throw exceptions, or can return a combined "Result" object containing result and optional exception
        private async Task<List<ExchangeRate>> GetRatesFromDataSource(IEnumerable<Currency> currenciesToLoad)
        {
            List<ExchangeRate> ratesFromDataSource;

            try
            {
                ratesFromDataSource = (await loader.GetExchangeRatesAsync(currenciesToLoad.Select(c => c.Code.ToString()), dateTimeService.GetNow())).ToList();
            }
            catch (Exception e)
            {
                logger.LogError("Failed loading rates from data source: {}", e.Message);
                ratesFromDataSource = [];
            }

            return ratesFromDataSource;
        }

        private void PutRatesToCache(IEnumerable<ExchangeRate> ratesFromDataSource)
        {
            DateTimeOffset expiration = loader.RateRefreshSchedule.GetNextRefreshTime();

            foreach (var loadedRate in ratesFromDataSource)
            {
                currencyCache.Set(loadedRate.SourceCurrency.Code, loadedRate, expiration);
                logger.LogInformation("Cached rate {} with expiration: {}", loadedRate, expiration);
            }
        }
    }
}
