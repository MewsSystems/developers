using System.Collections.Generic;
using ExchangeRateUpdater;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateSource
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}