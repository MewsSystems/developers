using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Queries;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;

/// <summary>
/// Handler for retrieving current exchange rates.
/// Returns the most recent rates for all active currency pairs.
/// Uses optimized database view vw_CurrentExchangeRates for performance.
/// </summary>
public class GetCurrentExchangeRatesQueryHandler : IQueryHandler<GetCurrentExchangeRatesQuery, IEnumerable<CurrentExchangeRateDto>>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetCurrentExchangeRatesQueryHandler> _logger;

    public GetCurrentExchangeRatesQueryHandler(
        ISystemViewQueries systemViewQueries,
        IDateTimeProvider dateTimeProvider,
        ILogger<GetCurrentExchangeRatesQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<CurrentExchangeRateDto>> Handle(
        GetCurrentExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting current exchange rates from optimized view");

        // Query optimized database view (single query with all joins)
        var viewResults = await _systemViewQueries.GetCurrentExchangeRatesAsync(cancellationToken);

        // Map to DTOs
        var currentRates = viewResults.Select(view =>
        {
            var daysOld = (int)(_dateTimeProvider.UtcNow - view.Created).TotalDays;

            return new CurrentExchangeRateDto
            {
                ProviderId = view.ProviderId,
                ProviderCode = view.ProviderCode,
                ProviderName = view.ProviderName,
                BaseCurrencyCode = view.BaseCurrencyCode,
                TargetCurrencyCode = view.TargetCurrencyCode,
                Rate = view.Rate,
                Multiplier = view.Multiplier,
                EffectiveRate = view.RatePerUnit,
                ValidDate = view.ValidDate,
                LastUpdated = view.Created,
                DaysOld = daysOld
            };
        }).ToList();

        _logger.LogDebug("Retrieved {Count} current exchange rates from view", currentRates.Count);

        return currentRates;
    }
}
