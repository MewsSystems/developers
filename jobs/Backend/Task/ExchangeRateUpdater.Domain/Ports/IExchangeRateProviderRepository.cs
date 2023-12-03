using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Ports;

public interface IExchangeRateProviderRepository
{
    Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates(DateTime exhangerRateDate);
    Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, DateTime From, DateTime To);
}