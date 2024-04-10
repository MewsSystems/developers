namespace ExchangeRates.Contracts.ExchangeRates;

public record ExchangeRateResponse(string SourceCode, string TargetCode, decimal Value);
