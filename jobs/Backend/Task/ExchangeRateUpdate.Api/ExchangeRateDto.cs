namespace ExchangeRateUpdate.Api;

public record ExchangeRateDto(string TargetCurrency, IEnumerable<string> SourceCurrencies);