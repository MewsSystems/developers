namespace ExchangeRateUpdater.Domain.Entities;

public class ReferenceTime
{
    public virtual DateTime GetTime() => DateTime.Now;
}
