namespace ExchangeRateUpdater.Interfaces
{
    public interface IClock
    {
        DateOnly Today { get; }
    }
}
