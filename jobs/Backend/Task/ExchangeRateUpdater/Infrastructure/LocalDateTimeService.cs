using System;
using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.Infrastructure;

internal class LocalDateTimeService : IDateTimeService
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }

    public DateTime GetNow()
    {
        return DateTime.Now;
    }

    public DateTime GetToday()
    {
        return DateTime.Today;
    }
}