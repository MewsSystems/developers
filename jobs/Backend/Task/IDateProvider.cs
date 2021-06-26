namespace ExchangeRateUpdater
{
    public interface IDateProvider
    {
        string GetCurrentDate(string format);
    }
}