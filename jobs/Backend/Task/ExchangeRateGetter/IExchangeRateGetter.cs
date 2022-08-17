using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater.ExchangeRateGetter
{
    // The idea behind this is to allow other Rate Getters in the future besides CNB
    public interface IExchangeRateGetter
    {
        Task<IEnumerable<ICurrencyDetails>> GetTodaysExchangeRates();
    }
}
