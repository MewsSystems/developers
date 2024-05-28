namespace ExchangeRateUpdater.Test;

public class MockDateTimeProvider(DateTime now) : IDateTimeProvider
{
    public DateTime Now => now;
}