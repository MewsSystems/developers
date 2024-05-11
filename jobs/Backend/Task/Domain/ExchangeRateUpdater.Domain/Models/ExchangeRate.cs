namespace ExchangeRateUpdater.Domain.Models;

public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value);