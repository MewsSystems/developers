using System;

namespace ExchangeRateUpdater.Repositories;

public record RateResource
{
    public int Amount { get; init; }
    public string Country { get; init; }
    public string Currency { get; init; }
    public string CurrencyCode { get; init; }
    public int Order { get; init; }
    public decimal Rate { get; init; }
    public DateOnly ValidFor { get; init; } // DateOnly
}
