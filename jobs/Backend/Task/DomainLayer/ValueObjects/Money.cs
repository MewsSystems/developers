namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents a monetary amount with its currency.
/// Immutable value object that ensures amounts are always associated with a currency.
/// </summary>
public record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a Money value object.
    /// </summary>
    /// <param name="amount">The monetary amount</param>
    /// <param name="currency">The currency</param>
    /// <exception cref="ArgumentNullException">Thrown when currency is null</exception>
    /// <exception cref="ArgumentException">Thrown when amount is negative</exception>
    public static Money Create(decimal amount, Currency currency)
    {
        if (currency == null)
            throw new ArgumentNullException(nameof(currency));

        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        return new Money(amount, currency);
    }

    /// <summary>
    /// Creates a zero money value for the specified currency.
    /// </summary>
    public static Money Zero(Currency currency) => new(0, currency);

    /// <summary>
    /// Checks if the amount is zero.
    /// </summary>
    public bool IsZero => Amount == 0;

    /// <summary>
    /// Adds two money values. Must be in the same currency.
    /// </summary>
    public Money Add(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot add money in different currencies: {Currency.Code} and {other.Currency.Code}");

        return new Money(Amount + other.Amount, Currency);
    }

    /// <summary>
    /// Subtracts two money values. Must be in the same currency.
    /// </summary>
    public Money Subtract(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot subtract money in different currencies: {Currency.Code} and {other.Currency.Code}");

        var newAmount = Amount - other.Amount;
        if (newAmount < 0)
            throw new InvalidOperationException("Result would be negative.");

        return new Money(newAmount, Currency);
    }

    /// <summary>
    /// Multiplies the amount by a factor.
    /// </summary>
    public Money Multiply(decimal factor)
    {
        if (factor < 0)
            throw new ArgumentException("Factor cannot be negative.", nameof(factor));

        return new Money(Amount * factor, Currency);
    }

    /// <summary>
    /// Divides the amount by a divisor.
    /// </summary>
    public Money Divide(decimal divisor)
    {
        if (divisor <= 0)
            throw new ArgumentException("Divisor must be positive.", nameof(divisor));

        return new Money(Amount / divisor, Currency);
    }

    /// <summary>
    /// Converts this money to another currency using the provided exchange rate.
    /// </summary>
    /// <param name="targetCurrency">The target currency</param>
    /// <param name="exchangeRate">The exchange rate from this currency to the target currency</param>
    public Money ConvertTo(Currency targetCurrency, ExchangeRateValue exchangeRate)
    {
        if (targetCurrency == null)
            throw new ArgumentNullException(nameof(targetCurrency));

        if (exchangeRate == null)
            throw new ArgumentNullException(nameof(exchangeRate));

        if (Currency == targetCurrency)
            return this;

        var convertedAmount = exchangeRate.Convert(Amount);
        return new Money(convertedAmount, targetCurrency);
    }

    /// <summary>
    /// Rounds the amount to the specified number of decimal places.
    /// </summary>
    public Money Round(int decimals = 2)
    {
        if (decimals < 0)
            throw new ArgumentException("Decimals must be non-negative.", nameof(decimals));

        var roundedAmount = Math.Round(Amount, decimals, MidpointRounding.AwayFromZero);
        return new Money(roundedAmount, Currency);
    }

    public override string ToString() => $"{Amount:F2} {Currency.Code}";

    // Operator overloads
    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money money, decimal factor) => money.Multiply(factor);
    public static Money operator /(Money money, decimal divisor) => money.Divide(divisor);

    // Comparison operators
    public static bool operator >(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money in different currencies.");
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money in different currencies.");
        return left.Amount < right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money in different currencies.");
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money in different currencies.");
        return left.Amount <= right.Amount;
    }
}
