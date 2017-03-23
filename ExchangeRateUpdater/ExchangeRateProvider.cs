using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IApiWrapper _fixexApi = new FixexApi();

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
            {
                throw new ArgumentException("Currencies cannot be null");
            }
            var currenciesSet = new HashSet<Currency>(currencies);

            if (currenciesSet.Count < 2)
            {
                return Enumerable.Empty<ExchangeRate>();
            }
            var exchangeRates = new List<ExchangeRate>();
            
            var tasks = currenciesSet.Select(cur => _fixexApi.Get(cur.Code, currenciesSet)).ToArray();

            Task.WaitAll(tasks.ToArray<Task>());

            foreach (var t in tasks)
            {
                var result = t.Result;
                if (result == null)
                {
                    continue;
                }
                exchangeRates
                    .AddRange(result.GetRates().Select(k => new ExchangeRate(new Currency(result.GetCurrencyCode()), new Currency(k.Key), k.Value)));
            }

            return exchangeRates;
        }
    }
}
