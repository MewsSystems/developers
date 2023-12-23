using Refit;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;

/// <summary>
/// See https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates.
/// </summary>
public interface ICnbApiClient
{
    [Get("/cnbapi/exrates/daily")]
    Task<CnbExchangeRatesDailyResponse> GetDailyExchangeRatesAsync(DateOnly date, string lang, CancellationToken cancellationToken);
}