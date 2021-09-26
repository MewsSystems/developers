using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProxy.Services
{
    public interface ICnbExchangeRatesService
    {
        Task<kurzy> GetAsync();
    }
}
