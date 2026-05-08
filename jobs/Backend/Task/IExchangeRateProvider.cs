using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        Task<string> GetExchangeRates(string date);
    }
}
