using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater
{
    public class SystemClock : IClock
    {
        public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
    }
}
