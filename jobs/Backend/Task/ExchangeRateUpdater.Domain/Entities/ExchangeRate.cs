﻿using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

public class ExchangeRate
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }
    public PositiveRealNumber CurrencyRate { get; }

    public ExchangeRate(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber? currencyRate)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        CurrencyRate = currencyRate ?? throw new ArgumentNullException(nameof(currencyRate));
    }
}