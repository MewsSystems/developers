using ExchangeRate.Api.Controllers.Models;
using ExchangeRate.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ExchangeRate.Api.Controllers
{
    /// <summary>
    /// Exchange Rates controller
    /// </summary>
    [ApiController]
    [Route("v1/ExchangeRates")]
    public class ExchangeRateNationalBankController : ControllerBase
    {

        private readonly ILogger<ExchangeRateNationalBankController> _logger;
        private readonly IExchangeRateService _exchangeRateService;

        /// <summary>
        /// ExchangeRates controller contructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="exchangeRateService"></param>
        public ExchangeRateNationalBankController(ILogger<ExchangeRateNationalBankController> logger, IExchangeRateService exchangeRateService)
        {
            _logger = logger;
            _exchangeRateService = exchangeRateService;
        }


        /// <summary>
        /// Retrieves exchange rates for a specific date.
        /// </summary>
        /// <param name="date">Date in format dd-MM-yyyy.</param>
        [HttpGet("date")]
        [ProducesResponseType(typeof(ExchangeRateBank), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDate([FromQuery] string date)
        {
            if (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return BadRequest("Invalid date format. Use dd-MM-yyyy.");
            }

            var rates = await _exchangeRateService.GetExchangeRatesByDay(dateTime);
            return Ok(rates);
        }

        /// <summary>
        /// Retrieves exchange rates for the day.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ExchangeRateBank), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDailyRates()
        {

            var rates = await _exchangeRateService.GetDailyExchangeRates();
            return Ok(rates);
        }
    }
}
