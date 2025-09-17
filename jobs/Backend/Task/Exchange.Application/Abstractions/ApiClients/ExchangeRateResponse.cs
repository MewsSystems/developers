namespace Exchange.Application.Abstractions.ApiClients;

public record ExchangeRateResponse(
    string ValidFor,
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    double Rate
);