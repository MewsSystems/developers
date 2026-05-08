namespace ExchangeRateUpdater;

public sealed record Currency(string Code)
{
    public override string ToString() => Code;
}
