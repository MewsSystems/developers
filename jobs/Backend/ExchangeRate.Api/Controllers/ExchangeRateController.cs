using AutoMapper;
using ExchangeRate.Api.Controllers.Models;
using ExchangeRate.Api.Models;
using ExchangeRate.Api.Validators;
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
        /// Retrieves exchange rates for the current day.
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
            catch (Exception)
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
        /// Retrieves exchange rates for a specific date.
        /// </summary>
        /// <param name="date">Date in format dd-MM-yyyy.</param>
        /// <param name="currency">Currency Code to be fetch ex: "USD".</param>
        [HttpGet("date")]
        [ProducesResponseType(typeof(Dictionary<string, ExchangeRateProviderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDay(
            [FromQuery, ValidateDateFormat] string date,
            [FromQuery, ValidateCurrency] string currency)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
                var provider = await _rateProviderService.GetExchangeRatesByDate(dateTime, new CurrencyDTO(currency.ToUpper()));
                var result = _mapper.Map<ExchangeRatesResultModel>(provider);
                return Ok(_mapper.Map<ExchangeRatesResultModel>(result).Results);
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentNullException)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Something went wrong!",
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        /// <summary>
        /// Retrieves exchange rates for two specific codes for a specific date.
        /// </summary>
        /// <param name="exchangeRate">Exchange Rate to be retrieved.</param>
        /// <returns>Dictionary containing exchange rates</returns>
        [HttpPost("currency")]
        [ProducesResponseType(typeof(Dictionary<string, ExchangeRateProviderModel>), StatusCodes.Status200OK)]
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

                return Ok(result.Results);
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentNullException)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (Exception)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Something went wrong!",
                    Status = StatusCodes.Status400BadRequest // fix this
                });
            }
        }
        /// <summary>
        /// Retrieves exchange rates for a list of currencies against CZK.
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
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception)
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
