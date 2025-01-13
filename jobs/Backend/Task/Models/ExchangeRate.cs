using System;

namespace ExchangeRateUpdater.Models;

public class ExchangeRate
{
    public Currency SourceCurrency { get; init; }
    
    public Currency TargetCurrency { get; init; }
    
    public decimal Value { get; init; }
    
    public DateTime ValidFor { get; init; }
}