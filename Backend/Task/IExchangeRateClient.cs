using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateClient
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}