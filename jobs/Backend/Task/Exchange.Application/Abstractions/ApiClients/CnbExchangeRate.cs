namespace Exchange.Application.Abstractions.ApiClients;

public record CnbExchangeRate(
    string ValidFor,
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    double Rate
);