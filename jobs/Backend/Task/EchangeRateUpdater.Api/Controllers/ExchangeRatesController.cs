namespace EchangeRateUpdater.Api.Controllers;

using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Application.Common.Models;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;
using ExchangeRateUpdater.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

public class ExchangeRatesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<List<ExchangeRateDto>>>> GetExchangeRatesByDate(
        [FromQuery] GetExchangesRatesByDateQuery query)
    {
        try
        {
            // Try cache first
            var cache = HttpContext.RequestServices.GetService<ICacheRepository>();
            if (cache is not null)
            {
                var cached = cache.GetFromCache<Result<List<ExchangeRateDto>>>("rates:latest");
                if (cached is not null)
                {
                    return Ok(cached);
                }
            }

            var result = await Mediator.Send(query);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // store in cache for later
            cache?.SetCache("rates:latest", result);
            cache?.SetCache("rates:latest:updatedAt", DateTimeOffset.UtcNow);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}