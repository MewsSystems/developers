namespace ExchangeRateUpdater.Client.Contracts;

public record Currency(string Code)
{
    public override string ToString()
    {
        return Code;
    }
}