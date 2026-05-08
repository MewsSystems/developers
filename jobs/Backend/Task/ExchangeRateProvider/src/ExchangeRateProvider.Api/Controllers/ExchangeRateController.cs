using ExchangeRateProvider.Api.Constants;
using ExchangeRateProvider.Api.Validators;
using ExchangeRateProvider.Application.Abstractions;
using ExchangeRateProvider.Application.DTOs;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Constants;
using ExchangeRateProvider.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProvider.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Tags("Exchange Rates")]
    public class ExchangeRateController(
        IQueryHandler<GetExchangeRatesQuery, 
        IList<ExchangeRateDto>> handler,
        IQuoteCurrenciesValidator quoteCurrenciesValidator, 
        ILogger<ExchangeRateController> logger) : ControllerBase
    {
        [HttpGet(ApiEndpoints.ExchangeRates.GetByCurrency)]
        [ProducesResponseType<IEnumerable<ExchangeRateDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get exchange rates")]
        [EndpointDescription("Retrieves current exchange rates for the specified base currency, with optional filtering by quote currencies.")]
        public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> GetByCurrency(
            [FromQuery] string? baseCurrency,
            [FromQuery] List<string>? quoteCurrencies,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Received request to get exchange rates. BaseCurrency: {BaseCurrency}, QuoteCurrencies: {QuoteCurrencies}",
                baseCurrency, quoteCurrencies is not null ? string.Join(", ", quoteCurrencies) : "null");

            if (string.IsNullOrWhiteSpace(baseCurrency))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid base currency",
                    Detail = "Base currency is required.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            if (!CurrencyServiceKeys.IsSupported(baseCurrency))
            {
                var supportedCurrencies = string.Join(", ", CurrencyServiceKeys.GetSupportedCurrencies());
                return BadRequest(new ProblemDetails
                {
                    Title = "Unsupported base currency",
                    Detail = $"The currency '{baseCurrency}' is not supported. Supported currencies: {supportedCurrencies}",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            if(quoteCurrencies is not null)
            {
                var validationResult = await quoteCurrenciesValidator.ValidateAsync(quoteCurrencies, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Invalid quote currencies",
                        Detail = errors,
                        Status = StatusCodes.Status400BadRequest
                    });
                }
            }

            var quotes = quoteCurrencies?
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(code => new Currency(code.ToUpperInvariant()))
                .ToList();

            var query = new GetExchangeRatesQuery(
                new Currency(baseCurrency),
                quotes?.Count > 0 ? quotes : null
            );

            var rates = await handler.HandleAsync(query, cancellationToken);
            return Ok(rates);
        }
    }
}
