using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProvider.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExchangeRatesController> _logger;
        private readonly IConfiguration _configuration;

        public ExchangeRatesController(
            IMediator mediator,
            ILogger<ExchangeRatesController> logger,
            IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Gets exchange rates for a specified list of currencies against CZK.
        /// </summary>
        /// <param name="currencyCodes">A comma-separated list of currency codes (e.g., USD,EUR,GBP).</param>
        /// <returns>A list of exchange rates against CZK.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates(
            [FromQuery] string currencyCodes)
        {
            // Parse currency codes
            var requestedCodes = currencyCodes.Split(',', StringSplitOptions.RemoveEmptyEntries);

            // Validate currency codes
            if (string.IsNullOrWhiteSpace(currencyCodes) || !requestedCodes.Any())
            {
                _logger.LogInformation("No currency codes provided, returning empty result.");
                return Ok(new List<ExchangeRate>());
            }

            // Enforce max currency count
            var maxCount = _configuration.GetValue<int>("ExchangeRateProvider:MaxCurrencies", 20);
            var requestedCount = requestedCodes.Length;

            if (requestedCount > maxCount)
            {
                _logger.LogWarning("Too many currency codes requested. Maximum allowed is {MaxCount}.", maxCount);
                return BadRequest($"Too many currency codes. Maximum allowed is {maxCount}.");
            }

            // Parse and validate currency codes
            var currencies = new List<Currency>();
            var invalidCodes = new List<string>();

            foreach (var code in requestedCodes.Select(c => c.Trim().ToUpper()).Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                try
                {
                    currencies.Add(new Currency(code));
                }
                catch (InvalidCurrencyCodeException)
                {
                    invalidCodes.Add(code);
                }
            }

            if (!currencies.Any())
            {
                _logger.LogWarning("No valid currency codes provided. Invalid codes: {InvalidCodes}", string.Join(", ", invalidCodes));
                return BadRequest($"No valid currency codes provided. Invalid codes: {string.Join(", ", invalidCodes)}");
            }

            if (invalidCodes.Any())
            {
                _logger.LogWarning("Some currency codes were invalid and ignored: {InvalidCodes}", string.Join(", ", invalidCodes));
            }

            // Only CZK is supported as the target currency
            var targetCurrency = new Currency("CZK");

            try
            {
                _logger.LogInformation(
                    "Fetching exchange rates for currency codes: {CurrencyCodes} against CZK.",
                    currencyCodes);

                var exchangeRates = await _mediator.Send(
                    new GetExchangeRatesQuery(currencies, targetCurrency));

                return Ok(exchangeRates);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error while fetching exchange rates: {Message}", ex.Message);
                return StatusCode(500, $"An application error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching exchange rates: {Message}", ex.Message);
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
