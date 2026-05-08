namespace ExchangeRateUpdater.Domain.Models;

public sealed class Currency(string code)
{
    public string Code { get; } = code ?? throw new ArgumentNullException(nameof(code));

    public override string ToString() => Code;
}