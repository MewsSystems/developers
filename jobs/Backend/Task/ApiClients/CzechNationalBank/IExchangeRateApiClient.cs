using ExchangeRateUpdater.ApiClients.Responses;
using Refit;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ApiClients.CzechNationalBank;

internal interface IExchangeRateApiClient
{
    /// <summary>
    /// Returns daily exchange rates
    /// </summary>
    [Get("/cnbapi/exrates/daily")]
    Task<ApiResponse<GetExchangeRatesResponse>> GetDaily();

    /// <summary>
    /// Returns "other" daily exchange rates for selected month
    /// </summary>
    /// <param name="yearMonth">Month in ISO format (yyyy-MM); default value: NOW</param>
    [Get("/cnbapi/fxrates/daily-month?yearMonth={yearMonth}")]
    Task<ApiResponse<GetExchangeRatesResponse>> GetOtherByYearMonth(string yearMonth);
}
