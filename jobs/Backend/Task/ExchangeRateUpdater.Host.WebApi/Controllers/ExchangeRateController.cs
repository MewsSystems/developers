using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Host.WebApi.Dtos;
using ExchangeRateUpdater.Host.WebApi.Mappers;
using ExchangeRateUpdater.Host.WebApi.Dtos.Request;
using ExchangeRateUpdater.Domain.UseCases;

namespace ExchangeRateUpdater.Host.WebApi.Controllers
{
    /// <summary>
    /// This controller handles exchange rates operations.
    /// </summary>
    [ApiController]
    [Route("/api/exchangeRates")]
    public class ExchangeRateController : ControllerBase
    {
        /// <summary>
        /// The port of exchange rate provider.
        /// </summary>
        private readonly IExchangeRateProviderRepository _exchangeRateUpdaterRepository;
        /// <summary>
        /// UseCase that handles exchange operations from one currency to the other.
        /// </summary>
        private readonly ExchangeUseCase _exchangeUseCase;

        /// <summary>
        /// Constructor to setup the ExchangeRateController.
        /// </summary>
        /// <param name="exchangeRateUpdaterRepository">The port for the exchange rate provider.</param>
        /// <param name="exchangeUseCase">The usecase to handle Exchange Orders.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExchangeRateController(IExchangeRateProviderRepository exchangeRateUpdaterRepository, ExchangeUseCase exchangeUseCase)
        {
            _exchangeRateUpdaterRepository = exchangeRateUpdaterRepository ?? throw new ArgumentNullException(nameof(exchangeRateUpdaterRepository));
            _exchangeUseCase = exchangeUseCase ?? throw new ArgumentNullException(nameof(exchangeUseCase));
        }


        /// <summary>
        /// Endpoint that gets all fx rates of a certain date.
        /// </summary>
        /// <param name="requestDate">Optional query parameter that will be used to query FX rates for a certain date.If not specified current date will be used.</param>
        /// <param name="cancellationToken">CancellationToken instance</param>
        /// <returns>The endpoint will return either fx rates for the specified/current date or if not present for the closest date earlier the specified one.</returns>
        [HttpGet("defaultRates")]
        public async Task<IActionResult> GetAllFxRates([FromQuery] DateTime? requestDate, CancellationToken cancellationToken)
        {
            var defaultCZKRates = await _exchangeRateUpdaterRepository.GetAllFxRates(requestDate ?? DateTime.Now.Date, cancellationToken);

            return Ok(defaultCZKRates.Select(ExchangeRateMapper.ToDto));
        }

        /// <summary>
        /// Endpoint that performs an exchange order with the specified, currencies, sum, and date.
        /// </summary>
        /// <param name="exchangeOrderDto">Exchange Order Dto containing all relevant information to perform the exchange like currencies and sum to exchange.</param>
        /// <param name="requestDate">Optional query parameter that will be used to query FX rates for a certain date.If not specified current date will be used.</param>
        /// <param name="cancellationToken">CancellationToken instance.</param>
        /// <returns>The exchange order result for either the specified date or if not existent the exchange result for an earlier date.</returns>
        [HttpPost("exchange")]
        public async Task<IActionResult> BuyAsync([FromBody] ExchangeOrderDto exchangeOrderDto, [FromQuery] DateTime? requestDate, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(exchangeOrderDto.SourceCurrency)) return BadRequest("Source Currency has to be specified.");
            if (string.IsNullOrWhiteSpace(exchangeOrderDto.TargetCurrency)) return BadRequest("Target Currency has to be specified.");
            if (exchangeOrderDto.SumToExchange.HasValue == false) return BadRequest("SumToExchange has to be specified.");

            var exchangeOrder = exchangeOrderDto.ToExchange();

            var exchangeResult = await _exchangeUseCase.ExecuteAsync(exchangeOrder, requestDate ?? DateTime.Now.Date, cancellationToken);

            if (exchangeResult == null) return NotFound("We do not support exchange rates for the mentioned source/target currencies.");

            return Ok(exchangeResult.ToExchangeResultDto());
        }
    }
}
