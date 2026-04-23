using ExchangeRateUpdater.Application.Common.Exceptions;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidRateFormatException : CustomValidationException
{
    public InvalidRateFormatException(string value) : base($"Invalid rate value: {value}.") { }
}