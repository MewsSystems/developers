using ApplicationLayer.Common.Abstractions;

namespace ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;

/// <summary>
/// Query to convert an amount from one currency to another using the latest exchange rate.
/// </summary>
public record ConvertCurrencyQuery(
    string SourceCurrencyCode,
    string TargetCurrencyCode,
    decimal Amount,
    int? ProviderId = null,
    DateOnly? Date = null) : IQuery<CurrencyConversionResult>;

/// <summary>
/// Result of a currency conversion operation.
/// </summary>
public record CurrencyConversionResult(
    decimal SourceAmount,
    decimal TargetAmount,
    string SourceCurrencyCode,
    string TargetCurrencyCode,
    decimal Rate,
    int Multiplier,
    decimal EffectiveRate,
    DateOnly ValidDate,
    int ProviderId,
    string ProviderName);
