using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.UseCases;
using ExchangeRateUpdaterApi.Dtos.Request;
using ExchangeRateUpdaterApi.Dtos.Response;
using ExchangeRateUpdaterApi.Mappers;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace ExchangeRateUpdaterApi.Controllers;

[ApiController]
[Route("/api/exchangerates")]
public class ExchangeRatesController : ControllerBase
{
    private readonly GetDailyExchangeRatesUseCase _getExchangeRatesUseCase;
    private readonly Logger _logger;
    
    public ExchangeRatesController(GetDailyExchangeRatesUseCase getExchangeRatesUseCase, Logger logger)
    {
        _getExchangeRatesUseCase = getExchangeRatesUseCase ?? throw new ArgumentNullException(nameof(getExchangeRatesUseCase));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("getExchangeRates")]
    public async Task<ActionResult> GetDailyExchangeRatesAsync([FromQuery] DateTime date, [FromBody] ExchangeRatesRequestDto exchangeRatesRequest, CancellationToken cancellationToken)
    {
        if (exchangeRatesRequest == null) 
            return BadRequest("Exchange rates to request should not be null");

        if (!exchangeRatesRequest.ExchangeRatesDetails.Any())
            return BadRequest("Exchange rates to request should not be empty.");
        
        var result = await _getExchangeRatesUseCase
            .ExecuteAsync(exchangeRatesRequest.ExchangeRatesDetails.Select(request => request.ToExchangeRateRequest()), 
                exchangeRatesRequest.DateToRequest,
                cancellationToken);
        
        return Ok(result.Select(exchangeRate => exchangeRate.ToExchangeRateResponseDto()).ToList()) ?? Ok(new List<ExchangeRateResultDto>());
    }
}