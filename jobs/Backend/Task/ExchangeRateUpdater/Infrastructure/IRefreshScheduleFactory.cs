using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Infrastructure.Config;

namespace ExchangeRateUpdater.Infrastructure;

internal interface IRefreshScheduleFactory
{
    IRateRefreshScheduler CreateRefreshSchedule(RefreshScheduleConfig config);
}
