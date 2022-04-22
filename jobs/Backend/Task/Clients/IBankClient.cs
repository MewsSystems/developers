using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Clients
{
    public interface IBankClient
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}
