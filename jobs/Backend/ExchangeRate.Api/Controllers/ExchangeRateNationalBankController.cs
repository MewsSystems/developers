using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Services.Interfaces;
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
        private readonly IExchangeRateProviderService _rateProviderService;

        /// <summary>
        /// ExchangeRates controller contructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="rateProviderService"></param>
        public ExchangeRateNationalBankController(ILogger<ExchangeRateNationalBankController> logger, IExchangeRateProviderService rateProviderService)
        {
            _logger = logger;
            _rateProviderService = rateProviderService;
        }


        /// <summary>
        /// Retrieves exchange rates for a specific date.
        /// </summary>
        /// <param name="date">Date in format dd-MM-yyyy.</param>
        /// <param name="currency">Currency Code to be fetch</param>
        [HttpGet("date")]
        [ProducesResponseType(typeof(Dictionary<string, ExchangeRateProviderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDate([FromQuery] string date, string currency)
        {
            if (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return BadRequest("Invalid date format. Use dd-MM-yyyy.");
            }
            
            var provider = await _rateProviderService.GetExchangeRatesByDate(dateTime, new CurrencyDTO(currency));
            return Ok(provider);
        }

        /// <summary>
        /// Retrieves exchange rates for the day.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExchangeRateProviderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDailyRates()
        {

            var provider = await _rateProviderService.GetExchangeRates();
            return Ok(provider);
        }

    }
}
