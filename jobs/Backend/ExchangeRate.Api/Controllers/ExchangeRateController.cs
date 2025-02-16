using AutoMapper;
using ExchangeRate.Api.Controllers.Models;
using ExchangeRate.Api.Models;
using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Exceptions;
using ExchangeRate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ExchangeRate.Api.Controllers
{
    /// <summary>
    /// Exchange Rates controller
    /// </summary>
    [ApiController]
    [Route("v1/exchange-rates")]
    public class ExchangeRateController : ControllerBase
    {

        private readonly ILogger<ExchangeRateController> _logger;
        private readonly IExchangeRateProviderService _rateProviderService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Exchange Rates
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="rateProviderService"></param>
        /// <param name="mapper"></param>
        public ExchangeRateController(ILogger<ExchangeRateController> logger,
                IExchangeRateProviderService rateProviderService,
                IMapper mapper)
        {
            _logger = logger;
            _rateProviderService = rateProviderService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves exchange rates for the day.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ExchangeRatesResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var provider = await _rateProviderService.GetExchangeRates();
                var result = _mapper.Map<ExchangeRatesResultModel>(provider);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "Something went wrong!",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        
        /// <summary>
        /// Retrieves exchange rates for a defined date.
        /// </summary>
        /// <param name="date">Date in format dd-MM-yyyy.</param>
        /// <param name="currency">Currency Code to be fetch ex: "USD".</param>
        [HttpGet("date")]
        [ProducesResponseType(typeof(ExchangeRatesResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDay([FromQuery] string date, string currency)
        {
            if (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Invalid date format. Use dd-MM-yyyy.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            try
            {
                var provider = await _rateProviderService.GetExchangeRatesByDate(dateTime, new CurrencyDTO(currency.ToUpper()));
                var result = _mapper.Map<ExchangeRatesResultModel>(provider);
                return Ok(_mapper.Map<ExchangeRatesResultModel>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Something went wrong!",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// For the date defined retrieves exchange rates for a two specific codes.
        /// </summary>
        /// <param name="exchangeRate">Exchange Rate to be retrieved.</param>
        /// <returns>Dictionary containing exchange rates</returns>
        [HttpPost("currency")]
        [ProducesResponseType(typeof(ExchangeRatesResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExchangeRate([FromBody] ExchangeRateModel exchangeRate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mapped = _mapper.Map<ExchangeRatesDTO>(exchangeRate);
                var result = _mapper.Map<ExchangeRatesResultModel>(await _rateProviderService.GetExchangeRatesByDate(mapped));
                if (result == null)
                    return NotFound(new ProblemDetails
                    {
                        Title = "Not Found",
                        Detail = "Source Currency not found. Try another currency.",
                        Status = StatusCodes.Status404NotFound
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Something went wrong!",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        /// <summary>
        /// For a defined date retrieves exchange rates for a list of currencies against CZK.
        /// </summary>
        /// <param name="currencyList">Currency Code List to be retrieved</param>
        /// <returns>Dictionary containing exchange rates</returns>
        [HttpPost("currencies")]
        [ProducesResponseType(typeof(ExchangeRatesResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDayCurrencies([FromBody] CurrenciesModel currencyList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var mapped = _mapper.Map<CurrenciesDTO>(currencyList);
                return Ok(_mapper.Map<ExchangeRatesResultModel>(await _rateProviderService.GetExchangeRates(mapped)));
            }
            catch (CurrencyNotFoundException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "Something went wrong!",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
}
