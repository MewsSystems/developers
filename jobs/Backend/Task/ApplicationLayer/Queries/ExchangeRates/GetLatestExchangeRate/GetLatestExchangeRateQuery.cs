using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;

/// <summary>
/// Query to get the most recent exchange rate for a specific currency pair.
/// </summary>
public record GetLatestExchangeRateQuery(
    string SourceCurrencyCode,
    string TargetCurrencyCode,
    int? ProviderId = null) : IQuery<ExchangeRateDto?>;
