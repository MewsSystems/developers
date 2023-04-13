using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Tests
{
    public class MockClock : IClock
    {
        public MockClock(DateOnly today)
        {
            Today = today;
        }

        public DateOnly Today { get; }
    }
}
