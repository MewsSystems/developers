namespace ExchangeRateUpdater.Application.DTOs;

public record ExchangeRateDto(string CurrencyCode, decimal RateToCZK);
public record RatesResponse(List<ExchangeRateDto> Rates, int TotalCount, DateTime UpdatedAt);
