using ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates;

namespace ExchangeRateDemo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController(ILogger<RatesController> logger, IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Query all available exchange rates
        /// </summary>
        /// <param name="date">Date in standard format: YYYY/MM/DD</param>
        /// <returns>List of exchange Rates</returns>
        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(typeof(IEnumerable<GetExchangeRatesResponse>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<GetExchangeRatesResponse>> GetAll([FromQuery] string? date)
        {
            logger.LogInformation("Getting all exchange rates");

            var result = await mediator.Send(new GetExchangeRatesQuery(date: date));

            return result;
        }

        /// <summary>
        /// Gets a filtered list of exchanges rates
        /// </summary>
        /// <param name="date">Date in standard format: YYYY/MM/DD</param>
        /// <param name="isoCodes">List of Isocodes to query</param>
        /// <returns>List of exchange Rates</returns>
        [HttpPost("GetFiltered")]
        [ProducesResponseType(typeof(IEnumerable<GetExchangeRatesResponse>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<GetExchangeRatesResponse>> GetFiltered([FromQuery] string? date, [FromBody]List<string> isoCodes)
        {
            isoCodes = isoCodes.ConvertAll(x => x.ToUpper());

            var result = await mediator.Send(new GetExchangeRatesQuery(isoCodes, date));

            return result;
        }
    }
}
