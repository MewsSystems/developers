using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Interfaces
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
