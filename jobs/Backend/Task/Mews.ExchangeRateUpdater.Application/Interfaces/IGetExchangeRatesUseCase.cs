using Mews.ExchangeRateUpdater.Domain.ValueObjects;

namespace Mews.ExchangeRateUpdater.Application.Interfaces;

public interface IGetExchangeRatesUseCase
{
    Task<IEnumerable<ExchangeRate>> ExecuteAsync(IEnumerable<Currency> currencies, CancellationToken ct);
}