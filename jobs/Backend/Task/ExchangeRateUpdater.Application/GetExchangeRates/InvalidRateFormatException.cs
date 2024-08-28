namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidRateFormatException : FormatException
{
    public InvalidRateFormatException(string value) : base($"Invalid rate value: {value}.") { }
}