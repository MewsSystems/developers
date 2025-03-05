using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Api.Dtos;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeRateUpdater.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ILogger<ExchangeRatesController> _logger;

        public ExchangeRatesController(IExchangeRateProvider exchangeRateProvider, ILogger<ExchangeRatesController> logger)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves exchange rates for a given date and set of currency codes.
        /// </summary>
        /// <param name="request">Request object containing date and comma-separated currency codes.</param>
        /// <returns>List of exchange rates or an empty array if no rates are available.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get exchange rates",
            Description = "Retrieves exchange rates for a given date and set of currency codes.",
            OperationId = "GetExchangeRates",
            Tags = new[] { "Exchange Rates" }
        )]
        public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> Get(
            [FromQuery] ExchangeRateRequestDto request)
        {
            if (!DateTime.TryParse(request.Date, out var requestedDate))
            {
                requestedDate = DateTime.UtcNow.Date; // Default to today's date if parsing fails.
            }

            var currencyCodes = request.Codes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim().ToUpperInvariant())
                .Distinct();

            var currencies = currencyCodes.Select(code => new Currency(code)).ToList();
            var result = new List<ExchangeRateDto>();

            try
            {
                await foreach (var rate in _exchangeRateProvider.GetExchangeRatesAsync(currencies, requestedDate))
                {
                    result.Add(ExchangeRateMapper.ToDto(rate));
                }

                if (result.Count == 0)
                {
                    _logger.LogInformation("No exchange rates found for {Date} and codes {Codes}.", requestedDate, request.Codes);
                    return Ok(Array.Empty<ExchangeRateDto>());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve exchange rates for {Date}.", requestedDate);
                return StatusCode(500, "An error occurred while retrieving exchange rates. Please try again later.");
            }
        }
    }
}
