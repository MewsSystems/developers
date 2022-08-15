using System;

namespace ExchangeRateUpdater;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Current => DateTime.Now;
}