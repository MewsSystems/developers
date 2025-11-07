using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Queries;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;

/// <summary>
/// Handler for retrieving all latest exchange rates across providers.
/// Returns the most recent rate (by ValidDate) for each currency pair, regardless of provider.
/// Uses optimized database view vw_AllLatestExchangeRates for performance.
/// </summary>
public class GetAllLatestExchangeRatesQueryHandler
    : IQueryHandler<GetAllLatestExchangeRatesQuery, IEnumerable<LatestExchangeRateDto>>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly ILogger<GetAllLatestExchangeRatesQueryHandler> _logger;

    public GetAllLatestExchangeRatesQueryHandler(
        ISystemViewQueries systemViewQueries,
        ILogger<GetAllLatestExchangeRatesQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _logger = logger;
    }

    public async Task<IEnumerable<LatestExchangeRateDto>> Handle(
        GetAllLatestExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting all latest exchange rates from optimized view");

        try
        {
            // Query optimized database view (single query with all joins)
            var viewResults = await _systemViewQueries.GetAllLatestExchangeRatesAsync(cancellationToken);

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
                FreshnessStatus = view.FreshnessStatus
            }).ToList();

            _logger.LogDebug("Retrieved {Count} latest exchange rates from view", latestRates.Count);

            return latestRates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all latest exchange rates from view");
            throw;
        }
    }
}
