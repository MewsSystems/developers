using Mews.ERP.AppService.Features.Fetch.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Mews.ERP.Api.Controllers;

/// <inheritdoc />
[ApiController]
[Route("/api/[controller]")]
public class FetchController : ControllerBase
{
    private readonly IFetchService fetchService;

    private readonly ILogger<FetchController> logger;
    
    /// <inheritdoc />
    public FetchController(IFetchService fetchService, ILogger<FetchController> logger)
    {
        this.fetchService = fetchService;
        this.logger = logger;
    }

    /// <summary>
    /// Lists the available exchange rates for the currencies stored in the internal database.
    /// </summary>
    /// <returns>A list of available exchange rates for currencies found in the database.</returns>
    [HttpGet]
    [ProducesResponseType(Status200OK)]
    public async Task<IActionResult> Fetch()
    {
        logger.LogInformation($"GET - Called 'Fetch' endpoint from the Exchange Rate Provider Api.");
        
        var rates = await fetchService.GetExchangeRatesAsync();
        
        return Ok(rates);
    }
}