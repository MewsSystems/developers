using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.Application.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using ExchangeRateUpdater.API.Validators;

namespace ExchangeRateUpdater.API.Controllers
{
    [ApiController]
    [Route("api/rates/")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExchangeRatesController> _logger;
        private readonly IValidator<GetExchangeRatesRequest> _getRatesValidator;

        public ExchangeRatesController(IMediator mediator, ILogger<ExchangeRatesController> logger, IValidator<GetExchangeRatesRequest> getRatesValidator)
        {
            _mediator = mediator;
            _logger = logger;
            _getRatesValidator = getRatesValidator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("")]
        public async Task<IActionResult> GetExchangeRates([FromQuery] GetExchangeRatesRequest request)
        {
            var result = await _getRatesValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest("Invalid Currency Supplied");
            }

            // TODO: outsource this conversion
            var targetCurrencies = new List<Currency>();

            foreach (string c in request.Currencies) 
            {
                var currency = new Currency(c);

                targetCurrencies.Add(currency);
            }

            // duplicate currencies ? > remove

            var exchangeRates = await _mediator.Send(new GetExchangeRatesQuery 
            {
                TargetCurrencies = targetCurrencies
            });

            return Ok(exchangeRates);
        }
    }
}
