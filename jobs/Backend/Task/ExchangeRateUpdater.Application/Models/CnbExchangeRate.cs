namespace ExchangeRateUpdater.Application.Models;

public sealed record CnbExchangeRate(string CurrencyCode, decimal Rate, int Amount);