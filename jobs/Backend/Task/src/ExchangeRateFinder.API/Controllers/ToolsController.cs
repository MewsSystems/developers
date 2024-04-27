using ExchangeRateFinder.Application;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateFinder.API.Controllers
{
    [ApiController]
    [Route("api/tools")]
    public class ToolsController : ControllerBase
    {
        private readonly IUpdateCZKExchangeRateDataService _updateCZKExchangeRateDataService;
        private readonly ILogger<ToolsController> _logger;

        public ToolsController(
            IUpdateCZKExchangeRateDataService updateCZKExchangeRateDataService,
            ILogger<ToolsController> logger)
        {
            _updateCZKExchangeRateDataService = updateCZKExchangeRateDataService;
            _logger = logger;
        }

        //That endpoint could be called via cron job on infrastructure level,
        //instead of what we have now as logic in the Program.cs about scheduling a task
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCzkExchangeRateData()
        {
            try
            {
                await _updateCZKExchangeRateDataService.UpdateDataAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
