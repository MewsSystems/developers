using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetDailyExchangeRates;
using ExchangeRateUpdater.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeRateUpdater.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
            [Produces("application/json")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> _logger;
        private readonly ISender _mediator;
        /// <summary>
        /// Constructor for the ExchangeRateController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ExchangeRateController(ILogger<ExchangeRateController> logger, ISender mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Fetches daily exchange rates from the central bank.
        /// </summary>
        [HttpGet]
        [SwaggerResponse(200, "Returns the daily exchange rates", typeof(IEnumerable<ExchangeRate>))]
        [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
        public async Task<IActionResult> GetDailyExchangeRates()
        {
            var command = new GetDailyExchangeRatesQuery();

            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                _logger.LogError(result.Errors.ToString());
                return Problem(result.Errors.First().Description);
            }

            return Ok(result.Value);

        }
    }
}
