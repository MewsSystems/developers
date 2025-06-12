using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using NodaTime;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateDataSource
{
    Task<ExchangeRateData> GetExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default);

    Task<bool> IsDataAvailableForDateAsync(LocalDate date,
        CancellationToken cancellationToken = default);
}