using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly List<IExchangeRatesSource> _exchangeRatesSources;

        public ExchangeRateProvider(IEnumerable<IExchangeRatesSource> exchangeRatesSources)
        {
            _exchangeRatesSources = exchangeRatesSources.ToList();
        }

        /// <summary>
        /// Returns exchange rates among the specified currencies that are defined by the <param name="currencies"/>.
        /// Returns only rates that are provided by the sources
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRate>();
            var currenciesLookup = currencies.ToLookup(x => x);
            
            foreach (var exchangeRatesSource in _exchangeRatesSources)
            {
                try
                {
                    result.AddRange(exchangeRatesSource.LoadExchangeRates()
                        .Where(x => currenciesLookup.Contains(x.SourceCurrency) && currenciesLookup.Contains(x.TargetCurrency)));
                }
                catch (Exception)
                {
                    var message = $"Error while loading data from '{exchangeRatesSource.GetType().Name}'";
                    Console.WriteLine(message);
                    throw;
                }   
            }
            
            return result;
        }
    }
}
