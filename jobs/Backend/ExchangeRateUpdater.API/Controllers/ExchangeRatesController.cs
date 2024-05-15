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
                BaseCurrency = request.BaseCurrency,
                TargetCurrencies = request.TargetCurrencies, // TODO this may not convert
                RoundingDecimal = request.RoundingDecimal
            });

            /*
            try
            {
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            return View();
            */

            return Ok(exchangeRates);
        }
    }
}
