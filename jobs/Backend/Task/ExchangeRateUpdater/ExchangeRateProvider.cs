using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Sources;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IEnumerable<IRateSource> _rateSources;

        public ExchangeRateProvider(IEnumerable<IRateSource> rateSources)
        {
            _rateSources = rateSources;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async IAsyncEnumerable<ExchangeRate> GetLatestExchangeRates(Currency targetCurrency, IEnumerable<Currency> sourceCurrencies)
        {
            // This ideally should also be cached in a structure similar to Dictionary<Currency, ExchangeRate>
            var today = DateOnly.FromDateTime(DateTime.Today);
            foreach (var rateSource in _rateSources)
            {
                var rates = await rateSource.GetRatesAsync(today);
                var matchingRates = rates.Where(x => x.TargetCurrency == targetCurrency && sourceCurrencies.Contains(x.SourceCurrency));
                foreach (var rate in matchingRates)
                {
                    yield return rate;
                }
            }
        }
    }
}
