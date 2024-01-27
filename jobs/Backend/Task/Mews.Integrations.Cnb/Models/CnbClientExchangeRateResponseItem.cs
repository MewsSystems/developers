namespace Mews.Integrations.Cnb.Models;

public abstract record CnbClientExchangeRateResponseItem
{
    public string ValidFor { get; init; } = null!;
    public int Order { get; init; }
    public string Country { get; init; } = null!;
    public string Currency { get; init; } = null!;
    public int Amount { get; init; }
    public string CurrencyCode { get; init; } = null!;
    public decimal Rate { get; init; }
}
