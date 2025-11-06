using DomainLayer.Common;
using DomainLayer.ValueObjects;

namespace DomainLayer.Aggregates.ExchangeRateAggregate;

/// <summary>
/// Aggregate root representing an exchange rate between two currencies.
/// Encapsulates rate validation and conversion logic.
/// </summary>
public class ExchangeRate : AggregateRoot<int>
{
    public int ProviderId { get; private set; }
    public int BaseCurrencyId { get; private set; }
    public int TargetCurrencyId { get; private set; }
    public int Multiplier { get; private set; }
    public decimal Rate { get; private set; }
    public DateOnly ValidDate { get; private set; }
    public DateTimeOffset Created { get; private set; }
    public DateTimeOffset? Modified { get; private set; }

    // EF Core constructor
    private ExchangeRate()
    {
    }

    private ExchangeRate(
        int providerId,
        int baseCurrencyId,
        int targetCurrencyId,
        int multiplier,
        decimal rate,
        DateOnly validDate)
    {
        if (providerId <= 0)
            throw new ArgumentException("Provider ID must be positive.", nameof(providerId));

        if (baseCurrencyId <= 0)
            throw new ArgumentException("Base currency ID must be positive.", nameof(baseCurrencyId));

        if (targetCurrencyId <= 0)
            throw new ArgumentException("Target currency ID must be positive.", nameof(targetCurrencyId));

        if (baseCurrencyId == targetCurrencyId)
            throw new ArgumentException("Base and target currencies cannot be the same.");

        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be positive.", nameof(multiplier));

        if (rate <= 0)
            throw new ArgumentException("Rate must be positive.", nameof(rate));

        ProviderId = providerId;
        BaseCurrencyId = baseCurrencyId;
        TargetCurrencyId = targetCurrencyId;
        Multiplier = multiplier;
        Rate = rate;
        ValidDate = validDate;
        Created = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new exchange rate.
    /// </summary>
    public static ExchangeRate Create(
        int providerId,
        int baseCurrencyId,
        int targetCurrencyId,
        int multiplier,
        decimal rate,
        DateOnly validDate)
    {
        return new ExchangeRate(
            providerId,
            baseCurrencyId,
            targetCurrencyId,
            multiplier,
            rate,
            validDate);
    }

    /// <summary>
    /// Reconstructs an ExchangeRate aggregate from persistence without validation or domain events.
    /// For use by infrastructure layer only when loading from database.
    /// </summary>
    public static ExchangeRate Reconstruct(
        int id,
        int providerId,
        int baseCurrencyId,
        int targetCurrencyId,
        int multiplier,
        decimal rate,
        DateOnly validDate,
        DateTimeOffset created,
        DateTimeOffset? modified)
    {
        return new ExchangeRate
        {
            Id = id,
            ProviderId = providerId,
            BaseCurrencyId = baseCurrencyId,
            TargetCurrencyId = targetCurrencyId,
            Multiplier = multiplier,
            Rate = rate,
            ValidDate = validDate,
            Created = created,
            Modified = modified
        };
    }

    /// <summary>
    /// Updates the rate value while maintaining validation.
    /// </summary>
    public void UpdateRate(decimal newRate, int newMultiplier)
    {
        if (newRate <= 0)
            throw new ArgumentException("Rate must be positive.", nameof(newRate));

        if (newMultiplier <= 0)
            throw new ArgumentException("Multiplier must be positive.", nameof(newMultiplier));

        Rate = newRate;
        Multiplier = newMultiplier;
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Converts an amount from base currency to target currency.
    /// </summary>
    public decimal ConvertAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        return (amount * Rate) / Multiplier;
    }

    /// <summary>
    /// Converts an amount from target currency to base currency (inverse operation).
    /// </summary>
    public decimal ConvertAmountInverse(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        return (amount * Multiplier) / Rate;
    }

    /// <summary>
    /// Checks if this rate is still valid based on the current date.
    /// </summary>
    public bool IsValidForDate(DateOnly date)
    {
        return ValidDate == date;
    }

    /// <summary>
    /// Checks if this rate is expired (valid date is in the past).
    /// </summary>
    public bool IsExpired => ValidDate < DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// Gets the effective rate per single unit (accounting for multiplier).
    /// </summary>
    public decimal EffectiveRate => Rate / Multiplier;
}
