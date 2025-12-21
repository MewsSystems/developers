namespace ExchangeRateProvider.Domain.Entities;

/// <summary>
/// Represents an exchange rate between two currencies.
/// </summary>
/// <param name="SourceCurrency">The source currency from which to convert.</param>
/// <param name="TargetCurrency">The target currency to which to convert.</param>
/// <param name="Value">The exchange rate value indicating how much of the target currency equals one
/// unit of the source currency.</param>
/// <param name="ValidFor">The date for which this exchange rate is valid.</param>
public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value, DateOnly ValidFor);
