using System.ComponentModel;
using System.Net;
using Asp.Versioning;
using ExchangeRateUpdater.API.Attributes;
using ExchangeRateUpdater.API.Configuration;
using ExchangeRateUpdater.Business.ExchangeRates;
using ExchangeRateUpdater.Dto.ExchangeRates;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeRateUpdater.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/exchange-rates")]
[ApiVersion("1.0")]
public class ExchangeRatesController(ExchangeRateProvider exchangeRateProvider) : ControllerBase
{
    [HttpGet("CZK")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Exchange rates", typeof(List<ExchangeRate>), ContentTypes.Json)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error", typeof(ProblemDetails),
        ContentTypes.ProblemJson)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Client error", typeof(ProblemDetails), ContentTypes.ProblemJson)]
    public async Task<List<ExchangeRate>> GetExchangeRates(
        [FromQuery, CurrencyCodesValidation, Description("comma-separated 3-letter uppercase codes (e.g., USD,EUR,GBP), ISO 4217")] string targetCurrencies)
    {
        return await exchangeRateProvider.GetExchangeRates(targetCurrencies.Split(',')
            .Distinct()
            .Select(a => new Currency(a))
            .ToList());
    }
}