using System;

namespace ExchangeRateUpdater.Models;

public record ExchangeRateResponseModel
{
    public long Amount { get; init; }
        
    public required string CurrencyCode { get; init; }
        
    public required decimal Rate { get; init; }
        
    public DateTime ValidFor { get; init; }
        
    public string Currency { get; init; }
}