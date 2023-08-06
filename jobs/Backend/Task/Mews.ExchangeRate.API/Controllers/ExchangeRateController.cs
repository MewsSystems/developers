using Ardalis.GuardClauses;
using AutoMapper;
using Mews.ExchangeRate.Domain;
using Mews.ExchangeRate.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mews.ExchangeRate.API.Controllers;

[ApiController]
[Route($"{ExchangeRateApiConstants.RoutePrefix}")]
[Produces("application/json")]
[SwaggerTag("Exchange rates")]
public class ExchangeRateController : ControllerBase
{
    private readonly ILogger<ExchangeRateController> _logger;
    private readonly IProvideExchangeRates _exchangeRatesProvider;
    private readonly IMapper _mapper;

    public ExchangeRateController(ILogger<ExchangeRateController> logger,
        IProvideExchangeRates exchangeRatesProvider, 
        IMapper mapper)
    {
        Guard.Against.Null(logger);
        Guard.Against.Null(exchangeRatesProvider);
        Guard.Against.Null(mapper);

        _logger = logger;
        _exchangeRatesProvider = exchangeRatesProvider;
        _mapper = mapper;
    }

    [SwaggerOperation(
        Summary = "Retrieves Exchange Rates",
        Description = "Returns exchange rates among the specified currencies that are defined by the source.",
        OperationId = "GetExchangeRates"
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of the exchange rates")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Returned when codes are not in ISO 4217 format")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Returned when an error happens when retrieving the list of exchange rates")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<Dtos.ExchangeRate>))]
    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] IEnumerable<Dtos.Currency> currencies)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        using (_logger.BeginScope(new Dictionary<string, object>()
        {
            ["currencies"] = currencies,
        }))
        {
            try
            {
                var domainCurrencies = _mapper.Map<IEnumerable<Domain.Currency>>(currencies);
                var exchangeRates = await _exchangeRatesProvider.GetExchangeRatesForCurrenciesAsync(domainCurrencies);
                var result = _mapper.Map<IEnumerable<Dtos.ExchangeRate>>(exchangeRates);
                return Ok(result);
            }
            catch (ExchangeRateException ex)
            {
                _logger.LogError(ex,
                    "An error occurred when getting the exchange rates for the currencies");
                return Problem();
            }
        }
    }
}
