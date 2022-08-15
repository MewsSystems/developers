using System;

namespace ExchangeRateUpdater;

public interface IDateTimeProvider
{
    DateTime Current { get; }
}