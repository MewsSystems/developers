using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.Interfaces
{
    public interface IWebProxyProvider
    {
        Task<string> GetUrlAsync(string url);
    }
}
