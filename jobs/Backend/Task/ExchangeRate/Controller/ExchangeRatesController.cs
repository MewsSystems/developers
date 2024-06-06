using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Controller.Model;
using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Controller
{
    /// <summary>
    /// API Controller for handling exchange rate requests.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExchangeRatesController"/> class.
    /// </remarks>
    /// <param name="exchangeRateService">Service to handle exchange rate operations.</param>
    /// <param name="logger">Logger instance for logging.</param>
    [ApiController]
    [Route("api/v1/exchange-rate")]
    public class ExchangeRatesController(IExchangeRateService exchangeRateService, ILogger<ExchangeRatesController> logger) : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
        private readonly ILogger<ExchangeRatesController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// API endpoint to fetch daily exchange rates for a given base currency and date in a specific language for a list of target currencies.
        /// </summary>
        /// <param name="baseCurrency">Base currency code (e.g., USD)</param>
        /// <param name="date">Date in 'yyyy-MM-dd' format</param>
        /// <param name="language">Language code ('EN' or 'CZ')</param>
        /// <param name="targetCurrencies">Comma-separated list of target currency codes</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>HTTP response with exchange rate data</returns>
        [HttpGet("daily-rate")]
        public async Task<IActionResult> GetDailyExchangeRates(
            [FromQuery][Required] string baseCurrency,
            [FromQuery][Required] string date,
            [FromQuery][Required] string language,
            [FromQuery][Required] string targetCurrencies,
            CancellationToken cancellationToken)
        {
            // Validate the date format
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest(new ErrorResponse("Invalid date format. Please use 'yyyy-MM-dd'."));
            }
            // Validate parsedDate is not later than today
            else if (parsedDate > DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest(new ErrorResponse("Invalid date. No exchange rate exists for a future date."));
            }

            // Validate the language code
            if (!Enum.TryParse<Language>(language, true, out var parsedLanguage))
            {
                return BadRequest(new ErrorResponse("Invalid language value. Please use 'EN' or 'CZ'."));
            }

            // Validate baseCurrency is not null or empty
            if (string.IsNullOrWhiteSpace(baseCurrency))
            {
                return BadRequest(new ErrorResponse("baseCurrency cannot be null or empty."));
            }
            
            // Validate targetCurrencies is not null or empty
            if (string.IsNullOrWhiteSpace(targetCurrencies))
            {
                return BadRequest(new ErrorResponse("Target currencies cannot be null or empty."));
            }

            // Split and trim the target currency codes
            var targetCurrencyCodes = Regex.Split(targetCurrencies, @"\s*,\s*")
                                           .Where(code => !string.IsNullOrWhiteSpace(code))
                                           .Select(code => code.Trim())
                                           .ToArray();

            // Convert the target currency codes to a list of Currency objects
            var targetCurrencyList = targetCurrencyCodes.Select(code => new Currency(code)).ToList();

            // Create the request object
            var request = new FetchDailyExchangeRateRequestInternal(new Currency(baseCurrency), parsedDate, parsedLanguage, targetCurrencyList);

            try
            {
                // Fetch the exchange rates
                var internalResponse = await _exchangeRateService.GetDailyExchangeRatesAsync(request, cancellationToken);

                // Map the internal response to the external response
                var externalResponse = new FetchDailyExchangeRateResponse(
                    internalResponse.SourceCurrency.Code,
                    internalResponse.Date,
                    internalResponse.ExchangeRates.Select(data =>
                        new ExchangeRateResponseItem(data.Currency, data.CurrencyCode, data.Country, data.Rate)
                    ).ToList());

                // Return the success response
                return Ok(new SuccessResponse<FetchDailyExchangeRateResponse>("Daily exchange rate fetched successfully", externalResponse));
            }
            catch (ExchangeRateUpdaterException ex)
            {
                _logger.LogInformation(ex, "An error occurred while fetching daily exchange rates.");
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}
