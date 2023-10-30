using Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services.Handlers.CzechNationalBank.Requests;
using Services.Handlers.CzechNationalBank.Response;
using System.Net;

namespace ExchangeRateUpdate.Controllers;

[ApiController]
[Route("controller")]
public class ExchangeRatesController : ControllerBase
{
    private readonly ILog<ExchangeRatesController> _logger;
    private readonly IMediator _mediator;

    public ExchangeRatesController(IMediator mediator, ILog<ExchangeRatesController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(Name = "getexchangerates")]
    public async Task<ActionResult> GetExchangeRates([FromBody] List<Currency> currencies)
    {
        try
        {
            _logger.Info("GetExchangeRates", currencies);
            return CreateContentResult(await _mediator.Send(new GetRateRequest(currencies)));
        }
        catch (Exception e)
        {
            _logger.Error($"GetExchangeRates", currencies, e);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    private ContentResult CreateContentResult(GetRateResponse rateResponse)
    {
        var content = JsonConvert.SerializeObject(rateResponse, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        return new ContentResult
        {
            ContentType = "application/json",
            Content = content,
            StatusCode = 200
        };
    }
}
