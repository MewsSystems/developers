namespace ExchangeRateUpdater.Domain.Models;

/// <summary>
/// Represents an exchange rate between two currencies.
/// </summary>
/// <param name="SourceCurrency">The base currency from which the exchange rate is calculated.</param>
/// <param name="TargetCurrency">The target currency to which the exchange rate applies.</param>
/// <param name="Value">The exchange rate value, representing how much of the target currency is obtained per one unit of the source currency.</param>
public sealed record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
{
    /// <summary>
    /// Returns a string representation of the exchange rate.
    /// </summary>
    /// <returns>A formatted string in the form "SOURCE/TARGET = VALUE".</returns>
    public override string ToString() => $"{SourceCurrency}/{TargetCurrency} = {Value}";
}
