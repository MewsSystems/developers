using ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Dto;
using ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Queries;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi
{
    /// <summary>
    /// Represents an interface for interacting with the Czech National Bank API to retrieve exchange rates.
    /// </summary>
    internal interface ICzechNationalBankApi
    {
        /// <summary>
        /// Retrieves daily exchange rates from the Czech National Bank API.
        /// </summary>
        /// <param name="queryParams">The query parameters for the exchange rate request.</param>
        /// <returns>A task representing the asynchronous operation, returning the response containing daily exchange rates.</returns>

        [Get("/cnbapi/exrates/daily")]
        Task<ExRateDailyResponse> GetExchangeRatesAsync(ExchangeRatesQuery queryParams);
    }
}
