namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidAmountFormatException : FormatException
{
    public InvalidAmountFormatException(string value) : base($"Invalid amount value: {value}.") { }
}