using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;

namespace ExchangeRateUpdater.Api.Application.Interfaces;

public interface IExchangeRateProvider
{
    Task<Result<IReadOnlyCollection<ExchangeRate>>> GetDailyRatesAsync(
        DateOnly? date = null,
        CancellationToken cancellationToken = default);
}
