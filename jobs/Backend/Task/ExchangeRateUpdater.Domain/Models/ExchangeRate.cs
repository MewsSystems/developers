namespace ExchangeRateUpdater.Domain.Models;

public sealed record ExchangeRate
{
    public required Currency SourceCurrency { get; init; }
    public required Currency TargetCurrency { get; init; }
    public required decimal Value { get; init; }

    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
}