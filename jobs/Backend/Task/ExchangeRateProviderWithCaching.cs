using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal class ExchangeRateProviderWithCaching : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        private List<ExchangeRate> _cachedExchangeRates = new List<ExchangeRate>();

        public ExchangeRateProviderWithCaching(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            if(!ContainsAllExchangeRates(currencies))
            {
                RemovePastExchangeRates();

                var exchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies, cancellationToken);

                AddNewExchangeRates(exchangeRates);
            }

            return _cachedExchangeRates.Where(r => currencies.Contains(r.SourceCurrency));
        }

        private void AddNewExchangeRates(IEnumerable<ExchangeRate> exchangeRates)
        {
            foreach (var exchangeRate in exchangeRates)
            {
                if (!_cachedExchangeRates.Contains(exchangeRate))
                {
                    _cachedExchangeRates.Add(exchangeRate);
                }
            }
        }

        private void RemovePastExchangeRates()
        {
            foreach(var exchangeRate in _cachedExchangeRates)
            {
                if(exchangeRate.Date < DateTime.Today)
                {
                    _cachedExchangeRates.Remove(exchangeRate);
                }
            }
        }

        private bool ContainsAllExchangeRates(IEnumerable<Currency> currencies)
        {
            return currencies.All(c => _cachedExchangeRates.Any(x => x.Equals(new ExchangeRate(c, Currency.CZK, DateTime.Today, 0, 0))));
        }
    }
}
