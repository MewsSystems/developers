using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.GetAllLatestUpdatedExchangeRates;

/// <summary>
/// Query to get all latest updated exchange rates across all providers.
/// Returns the most recently updated rate (by Created timestamp) for each currency pair.
/// Useful when multiple providers publish rates for the same ValidDate at different times.
/// </summary>
public record GetAllLatestUpdatedExchangeRatesQuery : IQuery<IEnumerable<LatestExchangeRateDto>>;
