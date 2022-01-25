using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.Interfaces
{
    public interface IQuotesProvider
    {
        Task<string> GetQuotesAsync();
    }
}
