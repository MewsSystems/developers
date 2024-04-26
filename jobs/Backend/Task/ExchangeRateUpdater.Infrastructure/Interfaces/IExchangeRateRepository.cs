using ExchangeRateFinder.Infrastructure.Models;

namespace ExchangeRateFinder.Infrastructure.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task<List<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate> GetByCodeAsync(string code);
        Task UpdateAllAsync(List<ExchangeRate> newData);
    }
}
 