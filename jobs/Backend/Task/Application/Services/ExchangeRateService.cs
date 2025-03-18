using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ExchangeRateUpdater.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateProvider _provider;

        public ExchangeRateService(IExchangeRateProvider provider)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
                throw new ArgumentException("No currencies provided."); // TODO: implement better error handling

            return await _provider.GetExchangeRatesAsync(currencies);

            // TODO: cache results
        }
    }
}