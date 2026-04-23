using ExchangeRateUpdater.Application.Common.Exceptions;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidExchangeRateLineFormatException : CustomValidationException
{
    public InvalidExchangeRateLineFormatException() : base("Exchange rate line format is different than expected. Unable to process.") { }   
}