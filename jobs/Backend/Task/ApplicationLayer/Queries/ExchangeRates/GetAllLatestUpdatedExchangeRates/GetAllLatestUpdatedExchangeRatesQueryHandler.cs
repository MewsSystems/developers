using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Queries;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetAllLatestUpdatedExchangeRates;

/// <summary>
/// Handler for retrieving all latest updated exchange rates across providers.
/// Returns the most recently updated rate (by Created timestamp) for each currency pair.
/// This is useful when different providers publish rates for the same ValidDate at different times,
/// as it always returns the freshest data from the database regardless of ValidDate.
/// Uses optimized database view vw_AllLatestUpdatedExchangeRates for performance.
/// </summary>
public class GetAllLatestUpdatedExchangeRatesQueryHandler
    : IQueryHandler<GetAllLatestUpdatedExchangeRatesQuery, IEnumerable<LatestExchangeRateDto>>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly ILogger<GetAllLatestUpdatedExchangeRatesQueryHandler> _logger;

    public GetAllLatestUpdatedExchangeRatesQueryHandler(
        ISystemViewQueries systemViewQueries,
        ILogger<GetAllLatestUpdatedExchangeRatesQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _logger = logger;
    }

    public async Task<IEnumerable<LatestExchangeRateDto>> Handle(
        GetAllLatestUpdatedExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting all latest updated exchange rates from optimized view");

        try
        {
            // Query optimized database view (single query with all joins)
            var viewResults = await _systemViewQueries.GetAllLatestUpdatedExchangeRatesAsync(cancellationToken);

            // Map to DTOs
            var latestRates = viewResults.Select(view => new LatestExchangeRateDto
            {
                Id = view.Id,
                ProviderId = view.ProviderId,
                ProviderCode = view.ProviderCode,
                ProviderName = view.ProviderName,
                BaseCurrencyId = view.BaseCurrencyId,
                BaseCurrencyCode = view.BaseCurrencyCode,
                TargetCurrencyId = view.TargetCurrencyId,
                TargetCurrencyCode = view.TargetCurrencyCode,
                Rate = view.Rate,
                Multiplier = view.Multiplier,
                EffectiveRate = view.RatePerUnit,
                ValidDate = view.ValidDate,
                Created = view.Created,
                Modified = view.Modified,
                DaysOld = view.DaysOld,
                FreshnessStatus = string.Empty, // Not available in this view
                MinutesSinceUpdate = view.MinutesSinceUpdate,
                UpdateFreshness = view.UpdateFreshness
            }).ToList();

            _logger.LogDebug("Retrieved {Count} latest updated exchange rates from view", latestRates.Count);

            return latestRates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all latest updated exchange rates from view");
            throw;
        }
    }
}
