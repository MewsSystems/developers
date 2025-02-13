using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.Api.Controllers
{
    [ApiController]
    [Route("v1/ExchangeRates")]
    public class ExchangeRateNationalBankController : ControllerBase
    {
        
        private readonly ILogger<ExchangeRateNationalBankController> _logger;
        private readonly IExchangeRatesService _exchangeRatesService;

        public ExchangeRateNationalBankController(ILogger<ExchangeRateNationalBankController> logger, IExchangeRatesService exchangeRatesService)
        {
            _logger = logger;
            _exchangeRatesService = exchangeRatesService;
        }

        [HttpGet("{day}/{month}/{year}")]
        [ProducesResponseType(typeof(ExchangeRateBank), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDate(int day, int month, int year)
        {
            var dateTime = new DateTime(year, month, day);
            var rates = await _exchangeRatesService.GetExchangeRatesByDay(dateTime);
            return Ok(rates);
        }
    }
}
