namespace Mews.ExchangeRateProvider;

/// <summary>
/// Represents an exchange rate
/// </summary>
/// <param name="SourceCurrency">The source currency</param>
/// <param name="TargetCurrency">The destination currency</param>
/// <param name="Value">The exchange rate for moving from source to target currency</param>
public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
{
    /// <summary>
    /// The string representation of this object
    /// </summary>
    /// <returns>Source currency followed by Target Currency and the exchange rate value to 9 decimal places</returns>
    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value:F9}";
}