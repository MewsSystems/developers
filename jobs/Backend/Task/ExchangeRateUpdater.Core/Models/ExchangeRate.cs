namespace ExchangeRateUpdater.Core.Models;

/// <summary>
/// Represents an exchange rate between two currencies for a specific date.
/// </summary>
/// <param name="SourceCurrency">The source currency</param>
/// <param name="TargetCurrency">The target currency</param>
/// <param name="Value">The exchange rate value</param>
/// <param name="Date">The date for which this rate is valid</param>
public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value, DateTime Date)
{
    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value} (Date: {Date:yyyy-MM-dd})";
    }
}
