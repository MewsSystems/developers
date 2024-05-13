using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Dtos;

namespace ExchangeRateUpdater.Infrastructure.Interfaces;

public interface IExchangeRateApi
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateOnly? date, Language? language);
}
