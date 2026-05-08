using System;

namespace ExchangeRateUpdater.Contracts;

public interface IRateRefreshScheduler
{
    DateTimeOffset GetNextRefreshTime();
}
