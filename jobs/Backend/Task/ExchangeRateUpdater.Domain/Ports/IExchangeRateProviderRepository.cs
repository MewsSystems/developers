using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Domain.Ports;

public interface IExchangeRateProviderRepository
{
    Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates();
}