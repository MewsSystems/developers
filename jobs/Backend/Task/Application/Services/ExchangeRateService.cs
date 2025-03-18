using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _provider.GetExchangeRatesAsync(currencies);
        }
    }
}