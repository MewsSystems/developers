using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.DTO;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRateDTO>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
