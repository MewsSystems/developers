using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateProxy
    {
        public Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(DateTimeOffset? date);
    }
}
