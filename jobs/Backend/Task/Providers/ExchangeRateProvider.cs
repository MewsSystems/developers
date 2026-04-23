using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IBankClient _bankClient;
        private readonly IMemoryCache _cache;

        public ExchangeRateProvider(IBankClient bankClient, IMemoryCache cache)
        {
            _bankClient = bankClient;
            _cache = cache;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var TargetCurrency = new Currency("CZK");
            var today = DateTime.Today;

            var exchangeRates = new List<ExchangeRate>();

            string cacheEntry = $"ExchangeRatesFor{today:yyyy-MM-dd}";

            if (_cache.TryGetValue(cacheEntry, out IEnumerable<ExchangeRate> cachedExchangeRates))
            {
                return cachedExchangeRates;
            }

            var bankExchangeRates = await _bankClient.GetExchangeRatesAsync();

            foreach (var currency in currencies)
            {
                var exchangeRate = bankExchangeRates.Rates.FirstOrDefault(er => er.CurrencyCode == currency.Code);

                if (exchangeRate == null) { continue; }

                exchangeRates.Add(new ExchangeRate(new Currency(exchangeRate.CurrencyCode),
                                                   TargetCurrency,
                                                   exchangeRate.Rate / exchangeRate.Amount));
            }

            AddExchangeRatesToCache(exchangeRates, today);

            return exchangeRates;
        }

        private void AddExchangeRatesToCache(List<ExchangeRate> exchangeRates, DateTime today)
        {
            string newCacheEntry = $"ExchangeRatesFor{today:yyyy-MM-dd}";

            _cache.Set(newCacheEntry, exchangeRates, new MemoryCacheEntryOptions() { AbsoluteExpiration = today.AddDays(1) });
        }
    }
}
