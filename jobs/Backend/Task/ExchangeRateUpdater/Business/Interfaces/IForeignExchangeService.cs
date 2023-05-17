using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business.Interfaces
{
    public interface IForeignExchangeService
    {
        /// <summary>
        /// Fetches today's exchange rates against the Czech Koruna (CZK) from a third party API.
        /// </summary>
        /// <returns></returns>
        Task<List<ThirdPartyExchangeRate>> GetLiveRatesAsync();
    }
}
