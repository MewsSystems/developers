using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Dtos;

namespace ExchangeRateUpdater.Infrastructure.Interfaces;

public interface IExchangeRateApi
{
    Task<IEnumerable<CnbExchangeRateResponseItem>> GetExchangeRatesAsync(DateOnly? date, Language? language);
}
