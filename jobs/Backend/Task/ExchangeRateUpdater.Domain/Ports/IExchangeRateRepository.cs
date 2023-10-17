using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Domain.Ports
{
    public interface IExchangeRateRepository
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(string defaultCurrency, IEnumerable<CurrencyCode> currencies);
    }
}
