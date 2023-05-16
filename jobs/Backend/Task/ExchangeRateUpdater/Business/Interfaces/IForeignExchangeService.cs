using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Business.Interfaces
{
    public interface IForeignExchangeService
    {
        IEnumerable<ExchangeRate> GetLiveRates();
    }
}
