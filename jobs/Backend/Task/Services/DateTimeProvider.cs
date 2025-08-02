using System;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
