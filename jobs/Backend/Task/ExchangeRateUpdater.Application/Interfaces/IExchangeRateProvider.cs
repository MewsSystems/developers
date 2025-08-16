using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Enums;

namespace ExchangeRateUpdater.Application.Interfaces;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateOnly? date, Language language);
}
