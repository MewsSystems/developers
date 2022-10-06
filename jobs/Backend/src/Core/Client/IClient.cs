using Core.Models;

namespace ExchangeRateUpdater.Client
{
    public interface IClient
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}
