using System;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Config;

namespace ExchangeRateUpdater.DataSources.RefreshSchedule;

internal class RefreshScheduleFactory : IRefreshScheduleFactory
{
    private readonly IDateTimeService dateTimeService;

    public RefreshScheduleFactory(IDateTimeService dateTimeService)
    {
        this.dateTimeService = dateTimeService;
    }

    public IRateRefreshScheduler CreateRefreshSchedule(RefreshScheduleConfig config)
    {
        return config == null
                ? new NoRefreshScheduler()
                : new DefaultRateRefreshScheduler(
                            TimeOnly.Parse(config.Time),
                            TimeZoneInfo.FindSystemTimeZoneById(config.TimeZone),
                            dateTimeService);

    }
}