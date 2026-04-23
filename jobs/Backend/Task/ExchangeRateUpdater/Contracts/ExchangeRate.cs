namespace ExchangeRateUpdater.Contracts;

/// <summary>
/// Exchange rate between two currencies.
/// </summary>
/// <param name="SourceCurrency">Source currency.</param>
/// <param name="TargetCurrency">Target currency.</param>
/// <param name="Value">Exchange rate, i.e. how many units of <see cref="TargetCurrency"/> is single unit of <see cref="SourceCurrency"/> worth.</param>
public record struct ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value);