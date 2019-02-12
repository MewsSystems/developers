using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IRateFeedSource feedSource;
        private readonly IRateFeedParser feedParser;

        public ExchangeRateProvider(IRateFeedSource feedSource, IRateFeedParser feedParser)
        {
            this.feedSource = feedSource;
            this.feedParser = feedParser;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            Task<IEnumerable<ExchangeRate>> task;
            try
            {
                task = GetExchangeRatesAsync(currencies);
                if(!task.IsCompleted)
                    task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException ?? ex;
            }

            if (task.IsFaulted)
            {
                throw task.Exception?.InnerException ?? task.Exception ?? new Exception("Asynchronous task for getting exchange rates failed without details.");
            }

            return task.Result;
        }

        /// <summary>
        /// Asynchronously loads exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source.
        /// </summary>
        /// <param name="currencies">Currencies we want exchange rates for.</param>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if(currencies == null)
                throw new ArgumentNullException(nameof(currencies));

            var currenciesRequested = currencies.ToArray();

            if(!currenciesRequested.Any())
                return Enumerable.Empty<ExchangeRate>();

            var feed = await feedSource.LoadRatesFeedAsync();
            if (string.IsNullOrWhiteSpace(feed))
                return Enumerable.Empty<ExchangeRate>();

            var rates = feedParser.Parse(feed);

            return rates.Where(x =>
                currenciesRequested.Contains(x.SourceCurrency) && currenciesRequested.Contains(x.TargetCurrency));
        }
    }
}
