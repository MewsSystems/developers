using ExchangeRateUpdater.Models.Entities;

namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateUpdaterService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime date);
}