namespace ExchangeRateUpdater.Common;

public interface IDateTimeSource
{
    DateTime UtcNow { get; } 
}