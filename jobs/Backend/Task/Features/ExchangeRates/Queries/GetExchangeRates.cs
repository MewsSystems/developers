using ExchangeRateUpdater.Common.Models;
using ExchangeRateUpdater.Features.ExchangeRates.Models;
using ExchangeRateUpdater.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Features.ExchangeRates.Queries;

public sealed class GetExchangeRatesController : ApiControllerBase
{
    [HttpGet]
    [Route("api/exchange-rates")]
    public async Task<IActionResult> GetAsync([FromQuery] DateOnly? date, [FromQuery] string? language, CancellationToken cancellationToken)
    {
        var results = await Mediator.Send(new GetExchangeRatesQuery(date, language), cancellationToken);
        return Ok(results);
    }
}

public sealed record GetExchangeRatesQuery(
    DateOnly? Date,
    string? Language) : IRequest<IEnumerable<ExchangeRate>>;

public sealed class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, IEnumerable<ExchangeRate>>
{
    private readonly IAPIClient _apiClient;
    private readonly ILogger<GetExchangeRatesQueryHandler> _logger;
    private readonly IExchangeRateProvider _provider;
    public GetExchangeRatesQueryHandler(
    IAPIClient apiClient,
    IExchangeRateProvider provider,
    ILogger<GetExchangeRatesQueryHandler> logger)
    {
        _apiClient = apiClient;
        _provider = provider;
        _logger = logger;
    }
    public async Task<IEnumerable<ExchangeRate>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calling exchange rate api.....");

        var rawData = await _apiClient.GetExchangeRatesDailyAsync(request.Date, request.Language, cancellationToken);

        return _provider.GetExchangeRates(Utils.SourceCurrencies, rawData);
    }
}