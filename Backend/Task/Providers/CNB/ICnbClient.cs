using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICnbClient
    {
        Task<IEnumerable<CnbXmlRow>> ReadExchangeRatesFromUrlAsync(string url);
    }
}