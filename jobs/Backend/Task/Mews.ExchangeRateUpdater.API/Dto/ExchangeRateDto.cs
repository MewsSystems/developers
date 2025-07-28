namespace Mews.ExchangeRateUpdater.API.Dto;

public record ExchangeRateDto(
    string Source,
    string Target,
    decimal Value
);