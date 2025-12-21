namespace ExchangeRateProvider.Api.Dto;

/// <summary>
/// Data Transfer Object representing an exchange rate for API responses.
/// </summary>
/// <param name="SourceCurrency">The ISO 4217 code of the source currency from which to convert.</param>
/// <param name="TargetCurrency">The ISO 4217 code of the target currency to which to convert.</param>
/// <param name="Value">The exchange rate value indicating how much of the target currency equals one
/// unit of the source currency.</param>
public record ExchangeRateDto(string SourceCurrency, string TargetCurrency, decimal Value);
