using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProxy.Implementation
{
    interface ICnbHttpClient
    {
        Task<string> GetXmlExchangeRatesAsync();
    }
}
