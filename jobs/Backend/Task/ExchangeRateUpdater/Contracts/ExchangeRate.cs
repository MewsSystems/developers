namespace ExchangeRateUpdater.Contracts;

/// <summary>
/// Exchange rate between two currencies.
/// </summary>
/// <param name="SourceCurrency">Source currency.</param>
/// <param name="TargetCurrency">Target currency.</param>
/// <param name="Value">Exchange rate, i.e. how many units of &lt;see cref="TargetCurrency"/&gt; is single unit of &lt;see cref="SourceCurrency"/&gt; worth.</param>
public record struct ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value);