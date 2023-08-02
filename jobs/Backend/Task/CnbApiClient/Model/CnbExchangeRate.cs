namespace CnbApiClient.Model;

public record CnbExchangeRate(
    int Amount,
    string Country,
    string Currency,
    string CurrencyCode,
    int Order,
    decimal Rate,
    string ValidFor
);