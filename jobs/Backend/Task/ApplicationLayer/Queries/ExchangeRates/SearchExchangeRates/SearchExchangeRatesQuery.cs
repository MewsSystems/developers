using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.SearchExchangeRates;

/// <summary>
/// Query to search exchange rates with multiple filtering options and pagination.
/// </summary>
public record SearchExchangeRatesQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SourceCurrencyCode = null,
    string? TargetCurrencyCode = null,
    int? ProviderId = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null,
    decimal? MinRate = null,
    decimal? MaxRate = null) : IQuery<PagedResult<ExchangeRateDto>>;
