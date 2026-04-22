namespace ExchangeRateUpdater.Domain;

public sealed class Currency
{
    public Currency(string code)
    {
        Code = code;
    }

    public string Code { get; }

    public override string ToString()
    {
        return Code;
    }
}
