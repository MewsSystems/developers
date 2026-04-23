namespace Exchange.Api.Dtos;

public record ExchangeRateDto(string SourceCurrency, string TargetCurrency, decimal Value);