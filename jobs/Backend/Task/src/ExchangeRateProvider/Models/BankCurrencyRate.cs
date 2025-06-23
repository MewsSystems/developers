namespace ExchangeRateProvider.Models;

public record BankCurrencyRate(
    long Amount,
    string CurrencyCode,
    decimal Rate);
