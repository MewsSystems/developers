using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateService
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string sourceCurrencyCode, IEnumerable<string> currencyCodes);
    }
}
