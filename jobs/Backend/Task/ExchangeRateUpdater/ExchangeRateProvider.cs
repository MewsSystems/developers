using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Repositories;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRatesRepository _exchangeRatesRepository;

        public ExchangeRateProvider(IExchangeRatesRepository exchangeRatesRepository)
        {
            _exchangeRatesRepository = exchangeRatesRepository;
        }


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            //currencies = currencies.ToList();
            if (!currencies.Any())
            {
                return [];
            }
            
            var rates = await _exchangeRatesRepository.GetExchangeRatesAsync();
            
            // generate a list of currency pairs to find rates for (USD, NZD), (USD,CZK) etc
            var currencyPairs = currencies
                .SelectMany(_ => currencies, (sourceCurrency, targetCurrency) => (sourceCurrency, targetCurrency))
                .Where(pair => pair.sourceCurrency !=  pair.targetCurrency);
            
            // filter out any currency pair that does not have a exchange rate 
            var result = currencyPairs
                .Select(currencyPair => rates.Find(rate => rate.SourceCurrency == currencyPair.sourceCurrency && rate.TargetCurrency == currencyPair.targetCurrency))
                .Where(exRate => exRate != null)
                .ToList();
            
            return result;
        }
        
    }
 
}
