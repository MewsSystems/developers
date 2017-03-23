using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal interface IApiWrapper
    {
        Task<IRate> Get(string baseCurrency, HashSet<Currency> currencies);
    }
}