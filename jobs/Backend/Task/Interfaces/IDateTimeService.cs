using System;

namespace ExchangeRateUpdater.Interfaces;

public interface IDateTimeService
{
    public DateTime UtcNow { get; }
}