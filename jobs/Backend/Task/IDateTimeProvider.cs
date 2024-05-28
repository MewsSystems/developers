using System;

namespace ExchangeRateUpdater;

public interface IDateTimeProvider
{
    public DateTime Now { get; }
}