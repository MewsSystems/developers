namespace ExchangeRateUpdater.Models;

public sealed record CnbExchangeRate(int Amount, string CurrencyCode, decimal Rate) { }