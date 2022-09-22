using System;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}