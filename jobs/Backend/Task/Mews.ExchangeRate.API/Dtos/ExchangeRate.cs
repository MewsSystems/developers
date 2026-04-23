namespace Mews.ExchangeRate.API.Dtos;

public record class ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, string Value);
