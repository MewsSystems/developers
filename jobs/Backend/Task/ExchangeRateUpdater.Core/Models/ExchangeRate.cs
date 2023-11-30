﻿namespace ExchangeRateUpdater.Core.Models;

public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }
    
    public ExchangeRate(string sourceCurrency, string targetCurrency, decimal value)
    {
        if (string.IsNullOrEmpty(sourceCurrency))
            throw new ArgumentException("Source currency cannot be empty or null.");
        
        if (string.IsNullOrEmpty(targetCurrency))
            throw new ArgumentException("Target currency cannot be empty or null.");
        
        SourceCurrency = new Currency(sourceCurrency);
        TargetCurrency = new Currency(targetCurrency);
        Value = value;
    }

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}