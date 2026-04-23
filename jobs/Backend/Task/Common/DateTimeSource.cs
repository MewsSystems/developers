namespace ExchangeRateUpdater.Common;

public class DateTimeSource : IDateTimeSource
{
    public DateTime UtcNow => DateTime.UtcNow;
}