using ExchangeRateUpdater.RateProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IRateProvider _rateProvider;

        public ExchangeRateProvider(IRateProvider rateProvider)
        {
            _rateProvider = rateProvider;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var rates = await _rateProvider.GetRatesAsync();
            var exchangeRates = new List<ExchangeRate>();

            foreach(var currency in currencies)
            {
                if (currency.Code == _rateProvider.BaseCurrency.Code)
                {
                    exchangeRates.Add(new ExchangeRate(currency, currency, 1));
                    continue;
                }

                var rate = rates.FirstOrDefault(r => string.Equals(r.Code, currency.Code, StringComparison.OrdinalIgnoreCase));                

                if(rate == null)
                {
                    continue;
                }

                exchangeRates.Add(new ExchangeRate(currency, _rateProvider.BaseCurrency, rate.Rate * decimal.Divide(_rateProvider.BaseAmmount, rate.Ammount)));
            }

            return exchangeRates;
        }
    }
}
