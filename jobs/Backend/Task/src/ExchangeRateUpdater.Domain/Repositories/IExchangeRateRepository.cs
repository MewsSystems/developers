using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<Dictionary<string, ExchangeRate[]>> FilterAsync(IEnumerable<Currency> currencies);
    Task<Dictionary<string, ExchangeRate[]>> GetAllAsync();
}