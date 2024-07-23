using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(ILogger<ExchangeRateController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetExchangeRates")]
        public async Task<IActionResult> Get()
        {
            return Ok("Initial Exchange Rate Controller Creation");
        }
    }
}
