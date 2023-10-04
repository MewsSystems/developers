namespace ExchangeRateUpdater;

public record Currency(string Code)
{
    public override string ToString() => Code;
}
