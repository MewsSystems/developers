using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business.Interfaces
{
    public interface IForeignExchangeService
    {
        Task<List<ThirdPartyExchangeRate>> GetLiveRatesAsync();
    }
}
