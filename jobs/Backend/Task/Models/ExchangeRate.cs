namespace ExchangeRateUpdater.Models;

/// <summary>
/// Represents an exchange rate between two currencies.
/// </summary>
/// <param name="SourceCurrency">The source currency.</param>
/// <param name="TargetCurrency">The target currency.</param>
/// <param name="Value">The exchange rate value (how many target currency units per 1 source currency unit).</param>
public record ExchangeRate(
    Currency SourceCurrency,
    Currency TargetCurrency,
    decimal Value)
{
    public override string ToString()
    {
        return $"{SourceCurrency.Code}/{TargetCurrency.Code}={Value}";
    }
}
