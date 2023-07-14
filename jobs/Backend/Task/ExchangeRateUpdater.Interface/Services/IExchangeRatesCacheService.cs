using ExchangeRateUpdater.Domain.Model;

namespace ExchangeRateUpdater.Interface.Services
{
    public interface IExchangeRatesCacheService
    {
        Task<IEnumerable<ExchangeRateEntity>?> GetOrCreateExchangeRatesAsync();
    }
}
