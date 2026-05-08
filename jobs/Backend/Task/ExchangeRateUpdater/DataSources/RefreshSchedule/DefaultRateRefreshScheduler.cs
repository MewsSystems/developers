using System;
using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.DataSources.RefreshSchedule;

internal class DefaultRateRefreshScheduler : IRateRefreshScheduler
{
    private readonly TimeOnly time;
    private readonly TimeZoneInfo timeZone;
    private readonly IDateTimeService dateTimeService;

    public DefaultRateRefreshScheduler(TimeOnly time, TimeZoneInfo timeZone, IDateTimeService dateTimeService)
    {
        this.time = time;
        this.timeZone = timeZone;
        this.dateTimeService = dateTimeService;
    }

    public DateTimeOffset GetNextRefreshTime()
    {
        var refreshTimeForCurrentDateInUtc =
            TimeZoneInfo.ConvertTimeToUtc(new DateTime(DateOnly.FromDateTime(dateTimeService.GetToday()), time, DateTimeKind.Unspecified), timeZone);

        DateTime nextRefreshDate = dateTimeService.GetUtcNow() < refreshTimeForCurrentDateInUtc ?
            refreshTimeForCurrentDateInUtc :
            refreshTimeForCurrentDateInUtc.AddDays(1);

        return nextRefreshDate;
    }

    public override string ToString() => $"{time:t} {timeZone.StandardName}";
}
