using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using NodaTime;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface ICacheService
{
    Task<ExchangeRateData?> GetExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default);

    Task SetExchangeRatesAsync(LocalDate date,
        ExchangeRateData data,
        CancellationToken cancellationToken = default);

    Task RemoveExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default);
}