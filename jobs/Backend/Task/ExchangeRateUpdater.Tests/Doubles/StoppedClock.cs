using System;
using Mews.Shared.Temporal;

namespace ExchangeRateUpdater.Tests.Doubles;

public record StoppedClock : IClock
{
    public DateTimeOffset Now { get; set; }
}
