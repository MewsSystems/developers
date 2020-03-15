using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateClient _exchangeRateClient;

        public ExchangeRateProvider(IExchangeRateClient exchangeRateClient)
        {
            _exchangeRateClient = exchangeRateClient;
        }
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var sourceRates = await _exchangeRateClient.GetExchangeRates();
            var combinations = AllCombinations(currencies.ToArray());
            return sourceRates.Intersect(combinations, new ExchangeRate.SourceTargetEqualityComparer()).ToArray();
        }

        private IEnumerable<ExchangeRate> AllCombinations(Currency[] currencies)
        {
            var result = new List<ExchangeRate>();
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    if(i != j)
                        result.Add(new ExchangeRate(currencies[i], currencies[j], 0));
                }
            }
            return result;
        }

        
    }
}
