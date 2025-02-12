using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Queries;
using ExchangeRateUpdater.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Handlers;

/// <summary>
/// Handles exchange rate queries using MediatR.
/// </summary>
public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, ExchangeRateResponse>
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<GetExchangeRatesQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetExchangeRatesQueryHandler"/> class.
    /// </summary>
    /// <param name="exchangeRateService">The exchange rate service responsible for fetching rates.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public GetExchangeRatesQueryHandler(IExchangeRateService exchangeRateService, ILogger<GetExchangeRatesQueryHandler> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the retrieval of exchange rates based on the query parameters.
    /// </summary>
    /// <param name="request">The query containing the requested date and currencies.</param>
    /// <param name="cancellationToken">A cancellation token to stop the operation if needed.</param>
    /// <returns>An <see cref="ExchangeRateResponse"/> containing the exchange rates for the requested date and currencies.</returns>
    public async Task<ExchangeRateResponse> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        var targetDate = request.Date ?? DateTime.Today;

        _logger.LogInformation(
            "Fetching exchange rates for date {Date} with currencies {Currencies}",
            targetDate.ToString("yyyy-MM-dd"), request.Currencies != null ? string.Join(", ", request.Currencies) : "All");

        var result = await _exchangeRateService.GetExchangeRatesAsync(targetDate, request.Currencies, cancellationToken);

        return result;
    }
}
