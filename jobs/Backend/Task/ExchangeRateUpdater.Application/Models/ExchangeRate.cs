namespace ExchangeRateUpdater.Application.Models;

public sealed record ExchangeRate(string SourceCurrency, string TargetCurrency, decimal Rate);