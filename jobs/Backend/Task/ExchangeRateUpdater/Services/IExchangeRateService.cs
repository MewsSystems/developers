using ExchangeRateUpdater.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateService
    {
        public Task<IEnumerable<IExchangeRate>> GetExchangeRatesAsync(string sourceCurrencyCode, IEnumerable<ICurrency> currencies);
    }
}
