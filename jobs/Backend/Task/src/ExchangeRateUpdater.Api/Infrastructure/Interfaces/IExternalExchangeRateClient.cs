using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;

namespace ExchangeRateUpdater.Api.Infrastructure.Interfaces;

public interface IExternalExchangeRateClient
{
    Task<Result<CnbExRateDailyResponse>> GetDailyRatesAsync(
        DateOnly? date = null,
        CancellationToken cancellationToken = default);
}
