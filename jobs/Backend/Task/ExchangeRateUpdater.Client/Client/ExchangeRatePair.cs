namespace ExchangeRateUpdater.Client.Client;

public record ExchangeRatePair(string Country, string Currency, decimal Amount, string Code, decimal Rate);