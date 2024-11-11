using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ICurrencyRateProvider _currencyRateProvider;

        public ExchangeRateProvider(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
        }

        // <summary>
        // Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        // by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        // do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        // some of the currencies, ignore them.
        // </summary>
        
        /// <summary>
        /// For all combinations of currencies specified by <paramref name="currencies"/> returns available exchange rates from underlying source. 
        /// </summary>
        /// <param name="currencies">Currencies to get exchange rates for.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Enumeration of available exchange rates.</returns>
        public async IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var currencySet = currencies.ToHashSet();

            var exchangeRates = await _currencyRateProvider.GetExchangeRatesAsync(cancellationToken);
            var exchangeRatesMap =
                exchangeRates.ToDictionary(x => new CurrencyCombination(x.SourceCurrency, x.TargetCurrency));

            foreach (var sourceCurrency in currencySet)
            {
                foreach (var targetCurrency in currencySet)
                {
                    if (sourceCurrency.Equals(targetCurrency))
                        continue;

                    var combination = new CurrencyCombination(sourceCurrency, targetCurrency);
                    if (exchangeRatesMap.TryGetValue(combination, out var exchangeRate))
                    {
                        yield return exchangeRate;
                    }
                }
            }
        }

        private record struct CurrencyCombination(Currency SourceCurrency, Currency TargetCurrency);
    }
}