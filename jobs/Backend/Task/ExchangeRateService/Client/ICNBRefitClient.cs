using ExchangeRateService.Client.Model.CNB;
using Refit;

namespace ExchangeRateService.Client;

public interface ICNBRefitClient
{
    [Get("/cnbapi/exrates/daily")]
    Task<ApiResponse<ExratesDailyResponse>> GetDailyRates([Query(Format = "yyyy-MM-dd")] DateTime date, [Query] string lang = "EN");
}