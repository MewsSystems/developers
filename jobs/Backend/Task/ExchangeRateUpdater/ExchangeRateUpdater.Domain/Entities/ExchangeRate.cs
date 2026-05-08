using System; 

namespace ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// Official Exchange Rate CNB (1 unit to CZK).
/// Immutable for thread-safety in API.
/// </summary>
public record ExchangeRate
{
    public string CurrencyCode { get; init; } = string.Empty;  // ISO4217 uppercase
    public decimal RateToCZK { get; init; } // Normalized rate /1
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow; 

    public ExchangeRate(string code, decimal rate, DateTime updatedAt)
    {
        CurrencyCode = code.ToUpperInvariant();
        RateToCZK = Math.Round(rate, 4);  // CNB precision
        UpdatedAt = updatedAt;
    }
}
