using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;

namespace ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Interfaces;

public interface ICnbApiClient
{
    Task<Result<CnbExRateDailyResponse>> GetDailyRatesAsync(
        DateOnly? date = null,
        CancellationToken cancellationToken = default);
}
