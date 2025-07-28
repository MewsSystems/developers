namespace Mews.ExchangeRateUpdater.Application.Exceptions;

public class NoDataForTodayException : Exception
{
    public NoDataForTodayException() : base("No data for today") { }
}