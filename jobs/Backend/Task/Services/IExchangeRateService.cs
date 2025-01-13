using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateService
{
    /// <summary>
    /// Fetches exchange rate data for a specific date and language.
    /// </summary>
    /// <param name="date">The date for which to fetch exchange rates (ISO format: yyyy-MM-dd).</param>
    /// <param name="language">The language for the response ("EN" or "CZ" (default)).</param>
    /// <returns>An <see cref="ExchangeRatesResponseModel"/> containing the exchange rate data.</returns>
    Task<ExchangeRatesResponseModel> GetExchangeRatesAsync(DateTime? date = null, string language = "EN");
}