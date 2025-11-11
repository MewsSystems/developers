using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.SearchExchangeRates;

public class SearchExchangeRatesQueryHandler
    : IQueryHandler<SearchExchangeRatesQuery, PagedResult<ExchangeRateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SearchExchangeRatesQueryHandler> _logger;

    public SearchExchangeRatesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<SearchExchangeRatesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<ExchangeRateDto>> Handle(
        SearchExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        // Get all rates - in a real implementation, this should be optimized with database-level filtering
        // For now, we'll filter in memory as a demonstration
        var providers = await _unitOfWork.ExchangeRateProviders.GetAllAsync(cancellationToken);
        var currencies = await _unitOfWork.Currencies.GetAllAsync(cancellationToken);

        var currencyDict = currencies.ToDictionary(c => c.Id, c => c.Code);
        var providerDict = providers.ToDictionary(p => p.Id, p => p.Name);

        // Get currency IDs for filtering
        int? sourceCurrencyId = null;
        int? targetCurrencyId = null;

        if (!string.IsNullOrWhiteSpace(request.SourceCurrencyCode))
        {
            var sourceCurrency = currencies.FirstOrDefault(c => c.Code == request.SourceCurrencyCode);
            if (sourceCurrency == null)
            {
                return PagedResult<ExchangeRateDto>.Create(
                    new List<ExchangeRateDto>(),
                    0,
                    request.PageNumber,
                    request.PageSize);
            }
            sourceCurrencyId = sourceCurrency.Id;
        }

        if (!string.IsNullOrWhiteSpace(request.TargetCurrencyCode))
        {
            var targetCurrency = currencies.FirstOrDefault(c => c.Code == request.TargetCurrencyCode);
            if (targetCurrency == null)
            {
                return PagedResult<ExchangeRateDto>.Create(
                    new List<ExchangeRateDto>(),
                    0,
                    request.PageNumber,
                    request.PageSize);
            }
            targetCurrencyId = targetCurrency.Id;
        }

        // Build query based on filters
        // Use database-level filtering where possible to optimize performance
        var startDate = request.StartDate ?? DateOnly.MinValue;
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        IEnumerable<DomainLayer.Aggregates.ExchangeRateAggregate.ExchangeRate> rates;

        if (sourceCurrencyId.HasValue && targetCurrencyId.HasValue)
        {
            // Currency pair specified - use history query
            rates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                sourceCurrencyId.Value,
                targetCurrencyId.Value,
                startDate,
                endDate,
                cancellationToken);
        }
        else if (request.ProviderId.HasValue)
        {
            // Provider specified but no currency pair - use provider+date range query
            rates = await _unitOfWork.ExchangeRates.GetByProviderAndDateRangeAsync(
                request.ProviderId.Value,
                startDate,
                endDate,
                cancellationToken);
        }
        else
        {
            // No currency pair or provider - this would be too broad
            // Return empty to avoid loading entire database
            _logger.LogWarning("Search requires either currency pair or provider to be specified");
            return PagedResult<ExchangeRateDto>.Create(
                new List<ExchangeRateDto>(),
                0,
                request.PageNumber,
                request.PageSize);
        }

        // Apply additional filters in memory (these are typically narrow after database filtering)
        var filteredRates = rates.AsEnumerable();

        // ProviderId filter only needed if we used currency pair query (not provider+date query)
        if (request.ProviderId.HasValue && sourceCurrencyId.HasValue && targetCurrencyId.HasValue)
        {
            filteredRates = filteredRates.Where(r => r.ProviderId == request.ProviderId.Value);
        }

        if (request.MinRate.HasValue)
        {
            filteredRates = filteredRates.Where(r => r.EffectiveRate >= request.MinRate.Value);
        }

        if (request.MaxRate.HasValue)
        {
            filteredRates = filteredRates.Where(r => r.EffectiveRate <= request.MaxRate.Value);
        }

        var totalCount = filteredRates.Count();

        // Apply pagination
        var pagedRates = filteredRates
            .OrderByDescending(r => r.ValidDate)
            .ThenByDescending(r => r.Created)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Map to DTOs
        var dtos = pagedRates.Select(r => new ExchangeRateDto
        {
            Id = r.Id,
            ProviderId = r.ProviderId,
            ProviderName = providerDict.TryGetValue(r.ProviderId, out var providerName) ? providerName : "Unknown",
            BaseCurrencyId = r.BaseCurrencyId,
            BaseCurrencyCode = currencyDict.TryGetValue(r.BaseCurrencyId, out var baseCurrencyCode) ? baseCurrencyCode : "UNKNOWN",
            TargetCurrencyId = r.TargetCurrencyId,
            TargetCurrencyCode = currencyDict.TryGetValue(r.TargetCurrencyId, out var targetCurrencyCode) ? targetCurrencyCode : "UNKNOWN",
            Rate = r.Rate,
            Multiplier = r.Multiplier,
            EffectiveRate = r.EffectiveRate,
            ValidDate = r.ValidDate,
            Created = r.Created,
            Modified = r.Modified
        }).ToList();

        _logger.LogInformation(
            "Search returned {Count} of {Total} exchange rates",
            dtos.Count,
            totalCount);

        return PagedResult<ExchangeRateDto>.Create(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
