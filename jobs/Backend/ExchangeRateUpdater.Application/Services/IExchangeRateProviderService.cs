using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateProviderService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}