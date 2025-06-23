
using ExchangeRateUpdater.Data.Responses;

namespace ExchangeRateUpdater.Data.Interfaces;
public interface IExchangeRateRepository
{
    Task<List<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date, CancellationToken cancellationToken);
}
