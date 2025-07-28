using Mews.ExchangeRateUpdater.Domain.ValueObjects;

namespace Mews.ExchangeRateUpdater.Domain.Interfaces;

public interface IExchangeRateRepository
{
    Task UpsertRatesAsync(IEnumerable<ExchangeRate> rates, DateTime date, CancellationToken ct);
    Task<IEnumerable<ExchangeRate>> GetRatesAsync(DateTime date, IEnumerable<Currency> currencies, CancellationToken ct);
    Task<bool> HasRatesForDateAsync(DateTime date, CancellationToken ct);
}