using ExchangeRateData.DataConnectors.Models;
using ExchangeRateData.DataConnectors.Repositories;
using ExchangeRateData.DataConnectors.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateData.Api.Controllers;

/// <summary>
/// Api methods related to exchange rates (for microservices)
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class ExchangeRateDataController : Controller
{
    private readonly ILogger _logger;
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly string _logName = "ExchangeRateDataController";


    public ExchangeRateDataController(ILogger<ExchangeRateDataController> logger, IExchangeRateRepository exchangeRateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _exchangeRateRepository = exchangeRateRepository ?? throw new ArgumentNullException(nameof(exchangeRateRepository));

    }


    /// <summary>
    /// Returns exchange rates from Czech National Bank for specified working day or today (if its working day and its after 14:30) otherwise last working day
    /// </summary>
    /// <param name="selectedDate">Date for which exchange rates are sought. In format dd.mm.yyyy or yyyy.mm.dd </param>
    /// <returns>ExchangeRate</returns>
    [HttpGet(Name = "GetExchangeRatesByCurrencyAndDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRatesByCurrencyAndDate(string? selectedDate, [FromQuery]string [] currencies)
    {
        string logHeader = _logName + ".GetExchangeRatesByCurrencyAndDate:";
        DateTime startTime = DateTime.Now;
        IEnumerable<ExchangeRate>? resultData;

        try
        {
            //get the date in proper format
            bool successFormatting = FormattingHelper.ParseToDateString(selectedDate, out string dateString);
            if (!successFormatting)
            {
                _logger.LogWarning($"{logHeader} Searched date was not in proper format: {selectedDate}");
                return BadRequest();
            }

            //get the data from third party
            resultData = await _exchangeRateRepository.GetExchangeRatesByCurrencyAndDateTaskAsync(dateString, currencies);
            TimeSpan timeMiddle = DateTime.Now - startTime;

            int count = resultData.Count();

            if (resultData == null || count < 1)
            {
                _logger.LogWarning($"{logHeader} Data was not found. Searched by date: {dateString}");
                return NotFound();
            }

            TimeSpan timeEnd = DateTime.Now - startTime;
            _logger.LogInformation($"{logHeader} Returning data - records: {count}, duration: {timeEnd}, middle: {timeMiddle}");
            return Ok(resultData);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }
}
