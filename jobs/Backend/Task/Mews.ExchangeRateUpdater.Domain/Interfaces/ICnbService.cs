using Mews.ExchangeRateUpdater.Domain.ValueObjects;

namespace Mews.ExchangeRateUpdater.Domain.Interfaces;

public interface ICnbService
{
    Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(CancellationToken ct);
}