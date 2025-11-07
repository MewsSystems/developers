using ExchangeRateUpdater.Contracts;
using System;

namespace ExchangeRateUpdater.Services
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
    }
}