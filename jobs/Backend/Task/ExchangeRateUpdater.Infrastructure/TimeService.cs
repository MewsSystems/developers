namespace ExchangeRateUpdater.Infrastructure;

public class TimeService : ITimeService
{
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}