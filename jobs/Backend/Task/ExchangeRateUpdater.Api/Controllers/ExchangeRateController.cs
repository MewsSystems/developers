using AutoMapper;
using ExchangeRateUpdater.API.Dtos;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Presentation.Controllers
{
    [ApiController]
    [Route("api/exchangeRates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IMapper _mapper;
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(
            IExchangeRateService exchangeRateService,
            IMapper mapper,
            ILogger<ExchangeRateController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns exchange rates.
        /// </summary>
        /// <param name="currencies">List of currencies to check exchange rates for</param>
        /// <returns>Exchange rates from CZK to requested currencies</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRateResponseDto>>> GetExchangeRates(
            [FromQuery]IEnumerable<string> currencies)
        {
            _logger.LogInformation("Received request for exchange rates");
            var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(_mapper.Map<IEnumerable<Currency>>(currencies));
            var result = _mapper.Map<IEnumerable<ExchangeRateResponseDto>>(exchangeRates);

            _logger.LogInformation("Returning request for exchange rates");

            return Ok(result);
        }
    }
}