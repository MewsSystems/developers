namespace ExchangeRateUpdater;

public record ExchangeRateLine(
    string Country,
    string Name,
    decimal Amount,
    string CurrencyCode,
    decimal ExchangeRate);