namespace ExchangeRateProvider.BankApiClients.Cnb.Models;

public record CnbBankCurrencyRate(
    string? ValidFor,
    int? Order,
    string? Country,
    string? Currency,
    long? Amount,
    string? CurrencyCode,
    decimal? Rate);