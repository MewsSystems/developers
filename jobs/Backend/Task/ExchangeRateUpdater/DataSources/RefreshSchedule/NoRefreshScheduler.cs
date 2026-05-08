using System;
using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.DataSources.RefreshSchedule;

internal sealed class NoRefreshScheduler : IRateRefreshScheduler
{
    public DateTimeOffset GetNextRefreshTime() => DateTimeOffset.MaxValue;

    public override string ToString() => "No refresh";
}
