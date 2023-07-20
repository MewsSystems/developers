using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateProviderService
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}