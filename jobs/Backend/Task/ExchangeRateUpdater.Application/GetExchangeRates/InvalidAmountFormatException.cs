using ExchangeRateUpdater.Application.Common.Exceptions;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidAmountFormatException : CustomValidationException
{
    public InvalidAmountFormatException(string value) : base($"Invalid amount value: {value}.") { }
}