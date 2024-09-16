namespace EchangeRateUpdater.Api.Controllers;

using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Application.Common.Models;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;
using Microsoft.AspNetCore.Mvc;

public class ExchangeRatesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<List<ExchangeRateDto>>>> GetExchangeRatesByDate(
        [FromQuery] GetExchangesRatesByDateQuery query)
    {
        try
        {
            var result = await Mediator.Send(query);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}