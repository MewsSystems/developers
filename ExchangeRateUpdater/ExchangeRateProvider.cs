using ExchangeRateUpdater.Factory.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IProviderFactory _factory;

        public ExchangeRateProvider(IProviderFactory factory)
        {
            _factory = factory;
        }
        
        /// <summary>
        /// Returns unique exchange rates for those of supplied currencies for which a source or target currency provider can be found.
        /// </summary>
        /// <param name="currencies">List of currencies</param>
        /// <returns>List of exchange rates</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currenciesList = currencies as IList<Currency> ?? currencies.ToList();

            var providers = _factory.GetProviders(currenciesList);
            
            return Task
                .WhenAll(providers
                    .Select(info => info.Provider.GetExchangeRatesAsync(info.BaseCurrency, currenciesList)))
                .GetAwaiter()
                .GetResult()
                .SelectMany(rates => rates)
                .Distinct();
        }

        /// <summary>
        /// Returns exchange rates for source currency and other currencies used as target.
        /// </summary>
        /// <param name="sourceCurrency">Source currency</param>
        /// <param name="currencies">Currencies for which the rates should be provided</param>
        /// <returns>List of exchange rates</returns>
        public IEnumerable<ExchangeRate> GetExchangeRatesForSourceCurrency(Currency sourceCurrency, IEnumerable<Currency> currencies)
        {
            var provider = _factory.GetSourceCurrencyProvider(sourceCurrency);
            return provider.GetExchangeRatesAsync(sourceCurrency, currencies)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns exchange rates for target currency and other currencies used as source.
        /// </summary>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="currencies">Currencies for which the rates should be provided</param>
        /// <returns>List of exchange rates</returns>
        public IEnumerable<ExchangeRate> GetExchangeRatesForTargetCurrency(Currency targetCurrency, IEnumerable<Currency> currencies)
        {
            var provider = _factory.GetTargetCurrencyProvider(targetCurrency);
            return provider.GetExchangeRatesAsync(targetCurrency, currencies)
                .GetAwaiter()
                .GetResult();
        }
    }
}
