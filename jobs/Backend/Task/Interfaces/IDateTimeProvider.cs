using System;

namespace ExchangeRateUpdater.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
