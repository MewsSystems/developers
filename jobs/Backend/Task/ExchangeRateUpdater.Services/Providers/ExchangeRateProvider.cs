using ExchangeRateUpdater.Abstractions.Contracts;
using ExchangeRateUpdater.Abstractions.Exceptions;
using ExchangeRateUpdater.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IRatesStore _ratesStore;

        public ExchangeRateProvider(IRatesStore ratesStore)
        {
            _ratesStore = ratesStore ?? throw new ArgumentNullException(nameof(ratesStore));
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies) 
        {
            if (currencies == null || currencies.Count() == 0)
                return new List<ExchangeRate>();

            var storeRates = _ratesStore.Get();
            if (storeRates?.Rates == null || storeRates.Rates.Count == 0)
                throw new UnavailableRatesException("No exchange rates are available at the store.");

            var requestedCurrencyCodes = 
                currencies.Select(c => c.Code.Trim().ToUpperInvariant()).Distinct();

            var storeRatesByCode = storeRates.Rates
                .ToDictionary(r => r.CurrencyCode.Trim().ToUpperInvariant(), r => r);

            var results = new List<ExchangeRate>();
            var czk = new Currency("CZK");

            foreach (var code in requestedCurrencyCodes)
            {
                if (!storeRatesByCode.TryGetValue(code, out var r))
                    continue;

                var amount = r.Amount > 0 ? r.Amount : 1m;
                var value = r.Rate / amount;

                results.Add(new ExchangeRate(czk, new Currency(code), value));
            }

            return results;
        }
    }
}
