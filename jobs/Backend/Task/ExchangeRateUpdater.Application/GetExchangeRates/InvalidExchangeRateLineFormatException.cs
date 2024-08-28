namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidExchangeRateLineFormatException : FormatException
{
    public InvalidExchangeRateLineFormatException() : base("Exchange rate line format is different than expected. Unable to process.") { }   
}