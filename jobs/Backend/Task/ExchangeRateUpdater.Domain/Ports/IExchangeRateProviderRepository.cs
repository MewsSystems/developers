using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Ports;

public interface IExchangeRateProviderRepository
{
    Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates();
    Task<ExchangeRate?> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency);
}