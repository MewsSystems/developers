using ExchangeRateUpdater.Application.Common.Exceptions;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidCurrencyCodeException : CustomValidationException
{
    public InvalidCurrencyCodeException(string currencyCode, ArgumentException innerException) : base($"Invalid currency code {currencyCode}.", innerException) { }
}