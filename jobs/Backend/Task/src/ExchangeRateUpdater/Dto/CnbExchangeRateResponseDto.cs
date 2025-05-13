namespace ExchangeRateUpdater.Dto;

public record CnbExchangeRate(
    DateOnly ValidFor,
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    decimal Rate
);

public record CnbExchangeRateResponseDto(
    IEnumerable<CnbExchangeRate> Rates
);