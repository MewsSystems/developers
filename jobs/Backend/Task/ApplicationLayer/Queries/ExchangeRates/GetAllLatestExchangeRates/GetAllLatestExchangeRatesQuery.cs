using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;

/// <summary>
/// Query to get all latest exchange rates across all providers.
/// Returns the most recent rate (by ValidDate) for each currency pair.
/// </summary>
public record GetAllLatestExchangeRatesQuery : IQuery<IEnumerable<LatestExchangeRateDto>>;
