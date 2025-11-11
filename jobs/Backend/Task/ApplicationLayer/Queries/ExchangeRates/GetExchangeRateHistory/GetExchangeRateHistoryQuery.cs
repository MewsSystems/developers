using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;

/// <summary>
/// Query to get historical exchange rates for a specific currency pair.
/// </summary>
public record GetExchangeRateHistoryQuery(
    string SourceCurrencyCode,
    string TargetCurrencyCode,
    DateOnly StartDate,
    DateOnly EndDate,
    int? ProviderId = null) : IQuery<IEnumerable<ExchangeRateHistoryDto>>;
