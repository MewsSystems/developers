using System;

namespace ExchangeRateUpdater.Contracts;

public interface IDateTimeService
{
    DateTime GetUtcNow();

    DateTime GetNow();

    DateTime GetToday();
}
