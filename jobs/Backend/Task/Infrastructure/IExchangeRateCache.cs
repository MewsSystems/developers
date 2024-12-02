using ExchangeRateUpdater.Entities;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
    public interface IExchangeRateCache
    {
        Task<ExchangeRate[]> GetCachedExchangeRatesAsync();
        Task SetExchangeRatesCacheAsync(ExchangeRate[] exchangeRates);
    }
}