using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.API.Controllers
{
    [ApiController]
    [Route("api/rates/")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExchangeRatesController> _logger;

        public ExchangeRatesController(IMediator mediator, ILogger<ExchangeRatesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetExchangeRates([FromQuery] GetExchangeRatesRequest request)
        {

            // some validation on the request


            // mediator
            var exchangeRates = await _mediator.Send(new GetExchangeRatesQuery 
            {
                TargetCurrencies = (IEnumerable<Application.Models.Currency>)request.TargetCurrencies, // TODO this may not convert
                RoundingDecimal = request.RoundingDecimal
            });

            return Ok(exchangeRates);
        }
    }
}
