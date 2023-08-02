namespace Domain.Model;

public record ExchangeRateDetails(
    int Amount,
    string Country,
    string Currency,
    string CurrencyCode,
    decimal Rate,
    string ValidFor
);