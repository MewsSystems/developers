using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Domain.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IReadOnlyCollection<Currency> currencies);
    }
}
