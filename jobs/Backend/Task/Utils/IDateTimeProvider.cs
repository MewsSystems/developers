using System;

namespace ExchangeRateUpdater.Utils;

public interface IDateTimeProvider
{
    DateTime UtcNow();
}