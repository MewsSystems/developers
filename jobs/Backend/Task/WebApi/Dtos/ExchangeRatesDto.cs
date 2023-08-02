namespace WebApi.Dtos;

public record ExchangeRatesDto(
    int Amount,
    string CurrencyCode,
    decimal Rate,
    string ValidFor
);
