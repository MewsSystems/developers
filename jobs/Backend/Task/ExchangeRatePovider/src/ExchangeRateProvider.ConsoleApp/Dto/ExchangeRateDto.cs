using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.ConsoleApp.Dto;

/// <summary>
/// Data Transfer Object representing an exchange rate for console application output.
/// </summary>
/// <param name="SourceCurrency">The source currency from which to convert.</param>
/// <param name="TargetCurrency">The target currency to which to convert.</param>
/// <param name="Value">The exchange rate value indicating how much of the target currency
/// equals one unit of the source currency.</param>
public record ExchangeRateDto(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
{
    /// <summary>
    /// Returns a formatted string representation of the exchange rate in the format
    /// "SourceCurrency/TargetCurrency=Value".
    /// </summary>
    /// <returns>A string in the format "SourceCurrency/TargetCurrency=Value".</returns>
    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
}
