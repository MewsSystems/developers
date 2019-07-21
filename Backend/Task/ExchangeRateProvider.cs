using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string ExchangeRateFixingSourceUrl = @"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string ExchangeRateOfOtherCurrenciesSourceUrl = @"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";

        private readonly IRateSourceParcer _rateSourceParcer;
        private readonly IRateSourceProvider _rateSourceProvider;

        public ExchangeRateProvider(IRateSourceParcer rateSourceParcer, IRateSourceProvider rateSourceProvider)
        {
            _rateSourceParcer = rateSourceParcer;
            _rateSourceProvider = rateSourceProvider;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException("currencies");

            IRateSourceProvider provider = new RateSourceProvider();

            List<string> exchangeRateSourceUrls = new List<string> {ExchangeRateFixingSourceUrl, ExchangeRateOfOtherCurrenciesSourceUrl};
            var exchangeRateRawSources = provider.GetRateSourcesByUrl(exchangeRateSourceUrls);
            var exchangeRateSource = _rateSourceParcer.ParceRateSource(exchangeRateRawSources);

            var rates = MatchExchangeRates(exchangeRateSource, currencies);

            return rates;
        }

        /// <summary>
        /// Create all the possible pairs from requested currencies. Match it with provided echange rate source
        /// </summary>
        /// <param name="source">Exchange rate source</param>
        /// <param name="requestedCurrencies">Requested currencies to match rates</param>
        /// <returns>Matched exchange rate pairs</returns>
        private IEnumerable<ExchangeRate> MatchExchangeRates(IEnumerable<ExchangeRate> source, IEnumerable<Currency> requestedCurrencies)
        {

            List<IEnumerable<Currency>> pairs = GetPermutations<Currency>(requestedCurrencies, 2).ToList();
            foreach (var pair in pairs)
            {
                var result = source.Where(s => s.SourceCurrency.Code == pair.ToList()[0].Code &&
                                               s.TargetCurrency.Code == pair.ToList()[1].Code).FirstOrDefault();
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        
        IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
