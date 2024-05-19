using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.API.Models.ResponseModels;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.Application.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

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

            var queryResult = await _mediator.Send(new GetExchangeRatesQuery 
            {
                Currencies = request.Currencies.Select(x => x.ToUpper()).ToList()
            });

            var response = new GetExchangeRatesResponse()
            {
                TargetCurrency = "CZK",
                Date = DateTime.Today.ToString("d"),
                ExchangeRates = queryResult.ExchangeRates.Select(x => x.ToString()).ToList()
            };

            return Ok(response);
        }
    }
}
