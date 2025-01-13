using System;

namespace ExchangeRateUpdater.Models;

public class ExchangeRate(
    Currency sourceCurrency, 
    Currency targetCurrency, 
    decimal value,
    DateTime validFor)
{
    public Currency SourceCurrency { get; } = sourceCurrency;

    public Currency TargetCurrency { get; } = targetCurrency;

    public decimal Value { get; } = value;

    public DateTime ValidFor { get; set; } = validFor;
}