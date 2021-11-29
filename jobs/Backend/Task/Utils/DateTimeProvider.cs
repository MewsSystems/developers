using System;

namespace ExchangeRateUpdater.Utils;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}