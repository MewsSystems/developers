using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.Interfaces;

public interface IExchangeRateApiClient
{
    Task<IEnumerable<CnbExchangeRate>> GetAllExchangeRatesAsync(CancellationToken cancellationToken);
}