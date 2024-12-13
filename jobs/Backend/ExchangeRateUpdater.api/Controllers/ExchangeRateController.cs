using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.Extensions;
using ExchangeRateUpdater.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExchangeRateUpdater.api.Controllers
{
    /// <summary>
    /// Controller responsible for handling exchange rate-related API requests.
    /// </summary>
    [ApiController]
    public class ExchangeRateController(IExchangeRateProvider exchangeRateProvider) : Controller
    {
        private readonly IExchangeRateProvider _exchangeRateProvider = exchangeRateProvider;
        private readonly char commaSeparator = ',';

        /// <summary>
        /// Retrieves daily exchange rates for the specified currencies and date.
        /// </summary>
        /// <param name="date">The optional date for the exchange rates. If not provided, the current date is used. Expected format: "yyyy-MM-dd".</param>
        /// <param name="currencies">A comma-separated list of currency codes (ISO 4217, three-letter codes) for which exchange rates are requested. Example: "USD,EUR,CZK".</param>
        /// <returns>An <see cref="IActionResult"/> containing the exchange rates for the specified currencies.</returns>
        [HttpGet(ExchangeRateRoutes.Daily)]
        public IActionResult GetDaily(string? date, string? currencies = null)
        {
            var result = _exchangeRateProvider.GetExchangeRates(ExchangeRateRoutes.Daily, date, currencies: currencies?.Split(commaSeparator).ToCurrency());
            return result.Match(
                    x => Ok(x.Select(x => new { Rate = x.ToString() })),
                    x => StatusCode((int)HttpStatusCode.BadRequest, x.Message));
        }

        /// <summary>
        /// Retrieves exchange rates for a specific currency for a given year and month.
        /// </summary>
        /// <param name="currency">The currency code (ISO 4217, three-letter code) for which exchange rates are requested. Example: "USD".</param>
        /// <param name="yearMonth">The year and month in "yyyy-MM" format for which exchange rates are requested. Example: "2024-11".</param>
        /// <returns>An <see cref="IActionResult"/> containing the exchange rates for the specified currency and year-month.</returns>
        [HttpGet(ExchangeRateRoutes.DailyCurrencyMonth)]
        public IActionResult GetDailyCurrencyMonth(string currency, string? yearMonth)
        {
            var result = _exchangeRateProvider.GetExchangeRates(ExchangeRateRoutes.DailyCurrencyMonth, yearMonth, currency);
            return result.Match(
                    x => Ok(x.Select(x => new { Rate = x.ToString() })),
                    x => StatusCode((int)HttpStatusCode.BadRequest, x.Message));
        }

        /// <summary>
        /// Retrieves exchange rates for a given year.
        /// </summary>
        /// <param name="year">The optional year for which exchange rates are requested. If not provided, the current year is used. Expected format: "yyyy".</param>
        /// <returns>An <see cref="IActionResult"/> containing the exchange rates for the specified year.</returns>
        [HttpGet(ExchangeRateRoutes.DailyYear)]
        public IActionResult GetDailyYear(string? year)
        {
            var result = _exchangeRateProvider.GetExchangeRates(ExchangeRateRoutes.DailyYear, year);
            return result.Match(
                    x => Ok(x.Select(x => new { Rate = x.ToString() })),
                    x => StatusCode((int)HttpStatusCode.BadRequest, x.Message));
        }
    }
}
