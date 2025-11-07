using System;

namespace ExchangeRateUpdater.Contracts
{
    public interface ITimeProvider
    {
        DateTimeOffset GetUtcNow();
    }
}