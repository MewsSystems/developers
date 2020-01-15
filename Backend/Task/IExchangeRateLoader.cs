using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateLoader
    {
        Task<IReadOnlyCollection<ExchangeRate>> LoadExchangeRates();
    }
}
