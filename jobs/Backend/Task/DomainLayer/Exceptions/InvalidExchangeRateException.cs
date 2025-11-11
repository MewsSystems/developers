namespace DomainLayer.Exceptions;

/// <summary>
/// Exception thrown when an exchange rate is invalid or violates business rules.
/// </summary>
public class InvalidExchangeRateException : DomainException
{
    public string? BaseCurrency { get; }
    public string? TargetCurrency { get; }

    public InvalidExchangeRateException(string message)
        : base(message)
    {
    }

    public InvalidExchangeRateException(string message, string baseCurrency, string targetCurrency)
        : base(message)
    {
        BaseCurrency = baseCurrency;
        TargetCurrency = targetCurrency;
    }

    public InvalidExchangeRateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
