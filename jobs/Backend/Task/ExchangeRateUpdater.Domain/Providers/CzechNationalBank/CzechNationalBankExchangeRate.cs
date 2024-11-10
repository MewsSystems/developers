namespace ExchangeRate.Domain.Providers.CzechNationalBank;

public sealed record CzechNationalBankExchangeRate(
    int Amount,
    string Country,
    int Order,
    DateTime ValidFor,
    string Currency,
    string CurrencyCode,
    decimal Rate);