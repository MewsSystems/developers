using ExchangeRate.Contracts.Models;

namespace ExchangeRate.Core.Providers.Interfaces;

public interface IExchangeRateProvider
{
    Task<IEnumerable<Contracts.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}
