using Refit;
using ExchangeRateUpdater.Infrastructure.CnbApi.Models;

namespace ExchangeRateUpdater.Infrastructure.CnbApi;

[Headers("Accept: application/json")]
public interface ICnbApi
{
    [Get("/cnbapi/exrates/daily")]
    Task<CnbExchangeRatesResponse> GetDailyRates([Query] string? date = null);
}