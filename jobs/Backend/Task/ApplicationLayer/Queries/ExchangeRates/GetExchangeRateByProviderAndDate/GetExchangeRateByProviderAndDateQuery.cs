using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.GetExchangeRateByProviderAndDate;

/// <summary>
/// Query to get all exchange rates from a specific provider for a given date.
/// </summary>
public record GetExchangeRateByProviderAndDateQuery(
    int ProviderId,
    DateOnly ValidDate) : IQuery<IEnumerable<ExchangeRateDto>>;
