using Mews.ExchangeRateProvider.Api.Utils;
using Mews.ExchangeRateProvider.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Mews.ExchangeRateProvider.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;

        private readonly IRateRepository _rateRepository;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="rateRepository"></param>
        /// <param name="logger"></param>
        public RatesController(IRateRepository rateRepository, ILogger<RatesController> logger)
        {
            _logger = logger;
            _rateRepository = rateRepository;
        }
        /// <summary>
        /// Get rates from CNB API with optional parameters.
        /// </summary>
        /// <param name="date" example="2023-11-15"></param>
        /// <param name="lang" example="CZ"></param>
        /// <param name="getAllRates" example="true"></param>
        /// <returns></returns>
        /// <remarks>
        /// Parameter description:<br/>
        /// date - Date in ISO format (yyyy-MM-dd); default value: today, Example : 2023-11-13<br/>
        /// lang - Language to display values EN, CZ, default value: CZ<br/>
        /// getAllRates - Filter results, true return all rates from CNB API, false return only filtered list, default is true<br/>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetExchangeRatesDaily([FromQuery] string? date, [FromQuery] string? lang, [FromQuery] bool? getAllRates)
        {
            // if client don't provide values for date or lang, we default them via helpers
            var validDate = DateTimeHelper.ParseDateFormat(date);
            var chosenLanguage = LanguageHelper.ParseLang(lang);    
            var fetchAllRates = getAllRates.HasValue ? getAllRates.Value : false;
            var dailyRates = await _rateRepository.GetDailyRatesAsync(validDate, chosenLanguage, fetchAllRates);
            return Ok(dailyRates);
        }
    }
}
