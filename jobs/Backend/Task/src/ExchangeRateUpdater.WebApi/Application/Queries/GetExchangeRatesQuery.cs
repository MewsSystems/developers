using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Queries
{
    public class GetExchangeRatesQuery
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public GetExchangeRatesQuery(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<IEnumerable<ExchangeRate>> ExecuteAsync(IEnumerable<Currency> currencies)
        {
            if(currencies == null || !currencies.Any()) throw new ArgumentException("Currencies must be provided", nameof(currencies));

            return await _exchangeRateProvider.GetExchangeRatesAsync(currencies);            
        }
    }
}
