using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(CancellationToken cancellationToken);
    }
}