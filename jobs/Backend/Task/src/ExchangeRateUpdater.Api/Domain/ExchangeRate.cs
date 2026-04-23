using ExchangeRateUpdater.Api.Domain.Common;

namespace ExchangeRateUpdater.Api.Domain;

/// <summary>
/// Represents an exchange rate between two currencies.
/// The rate expresses how many units of the target currency equal one unit of the source currency.
/// </summary>
public sealed record ExchangeRate
{
    private ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal rate, int amount, DateOnly validFor)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Rate = rate;
        Amount = amount;
        ValidFor = validFor;
    }

    /// <summary>
    /// The currency being converted from.
    /// </summary>
    public Currency SourceCurrency { get; }

    /// <summary>
    /// The currency being converted to (always CZK for CNB rates).
    /// </summary>
    public Currency TargetCurrency { get; }

    /// <summary>
    /// The exchange rate value: 1 unit of SourceCurrency equals this many units of TargetCurrency.
    /// </summary>
    public decimal Rate { get; }

    /// <summary>
    /// The number of units of the source currency the rate applies to.
    /// </summary>
    public int Amount { get; }

    /// <summary>
    /// The date for which this exchange rate is valid.
    /// </summary>
    public DateOnly ValidFor { get; }

    /// <summary>
    /// Creates a new exchange rate with validation.
    /// </summary>
    public static Result<ExchangeRate> Create(string sourceCurrencyCode, string targetCurrencyCode, decimal rate, int amount, DateOnly validFor)
    {
        var sourceResult = Currency.Create(sourceCurrencyCode);
        if (!sourceResult.IsSuccess)
            return Result<ExchangeRate>.Failure(sourceResult.Error);

        var targetResult = Currency.Create(targetCurrencyCode);
        if (!targetResult.IsSuccess)
            return Result<ExchangeRate>.Failure(targetResult.Error);

        if (rate < 0)
            return Result<ExchangeRate>.Failure(ExchangeRateErrors.InvalidRate(rate));

        if (amount <= 0)
            return Result<ExchangeRate>.Failure(ExchangeRateErrors.InvalidAmount(amount));

        return Result<ExchangeRate>.Success(new ExchangeRate(sourceResult.Value, targetResult.Value, rate, amount, validFor));
    }

    public override string ToString() => FormattableString.Invariant($"{SourceCurrency}/{TargetCurrency}={Rate}");
}
