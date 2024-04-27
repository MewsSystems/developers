using ExchangeRateFinder.API.Mappers;
using ExchangeRateFinder.API.RequestModels;
using ExchangeRateUpdater.Application;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateFinder.API.Controllers
{
    [ApiController]
    [Route("api/exchange-rates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ICalculatedExchangeRateResponseMapper _calculatedExchangeRateResponseMapper;
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(
            IExchangeRateService exchangeRateService,
            ICalculatedExchangeRateResponseMapper calculatedExchangeRateResponseMapper,
            ILogger<ExchangeRateController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _calculatedExchangeRateResponseMapper = calculatedExchangeRateResponseMapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetExchangeRates([FromQuery] ExchangeRateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var currencyCodes = request.TargetCurrencyCodes.Split(',');
                var exchangeRates = await _exchangeRateService.GetExchangeRates(request.SourceCurrencyCode, currencyCodes);
                var mappedExchangeRates = exchangeRates.Select(e => _calculatedExchangeRateResponseMapper.Map(e)).ToList();

                return Ok(mappedExchangeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
