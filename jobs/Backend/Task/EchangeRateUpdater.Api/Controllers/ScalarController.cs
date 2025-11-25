namespace EchangeRateUpdater.Api.Controllers;

using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ScalarController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> Get()
    {
        var cache = HttpContext.RequestServices.GetService<ICacheRepository>();
        if (cache is null)
        {
            return Ok(new { count = 0, lastUpdated = (DateTimeOffset?)null });
        }

        var cached = cache.GetFromCache<List<ExchangeRateDto>>("rates:latest");
        var updated = cache.GetFromCache<DateTimeOffset?>("rates:latest:updatedAt");

        var count = cached?.Count ?? 0;

        return Ok(new { count, lastUpdated = updated });
    }
}
