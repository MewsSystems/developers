namespace ExchangeRateUpdater.Models.Types;

/// <summary>
/// Represents an exchange rate between two currencies.
/// </summary>
internal record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, Rate Rate);