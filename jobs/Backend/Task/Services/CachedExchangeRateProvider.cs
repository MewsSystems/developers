using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class CachedExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider origin;
        private readonly IMemoryCache memoryCache;

        public CachedExchangeRateProvider(IExchangeRateProvider origin, IMemoryCache memoryCache)
        {
            this.origin = origin ?? throw new ArgumentNullException(nameof(origin));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var notCachedCurrencies = new List<Currency>();
            var cachedExchangeRates = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                ExchangeRate cachedExchangeRate;
                if (!memoryCache.TryGetValue(currency.Code, out cachedExchangeRate))
                {
                    notCachedCurrencies.Add(currency);
                }
                else
                {
                    cachedExchangeRates.Add(cachedExchangeRate);
                }
            }

            if (!notCachedCurrencies.Any() || AreCurrenciesBlocked(notCachedCurrencies))
            {
                return cachedExchangeRates;
            }

            var exchangeRates = await origin.GetExchangeRates(notCachedCurrencies);
            var currencyCodes = exchangeRates.Select(x => x.SourceCurrency.Code);
            SetToCache(exchangeRates);
            BlockCurrencies(notCachedCurrencies
                .Select(x => x.Code)
                .Where(x => !currencyCodes.Contains(x)));

            return cachedExchangeRates.Concat(exchangeRates);
        }

        private void SetToCache(IEnumerable<ExchangeRate> exchangeRates)
        {
            foreach (var exchangeRate in exchangeRates)
            {
                // TODO: choose correct absoluteExpiration (e.g. next day at 14-30)
                memoryCache.Set(exchangeRate.SourceCurrency.Code, exchangeRate, DateTimeOffset.Now.AddMinutes(1));
            }
        }

        private void BlockCurrencies(IEnumerable<string> currencyCodes)
        {
            foreach (var currencyCode in currencyCodes)
            {
                // TODO: choose correct absoluteExpiration (e.g. next day at 14-30)
                memoryCache.Set($"Blocked:{currencyCode}", true, DateTimeOffset.Now.AddMinutes(1));
            }
        }

        private bool AreCurrenciesBlocked(IEnumerable<Currency> currencies)
        {
            bool value;

            return currencies.All(currency => memoryCache.TryGetValue($"Blocked:{currency.Code}", out value));
        }
    }
}
