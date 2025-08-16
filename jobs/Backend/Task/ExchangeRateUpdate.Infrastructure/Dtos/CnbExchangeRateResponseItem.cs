namespace ExchangeRateUpdater.Infrastructure.Dtos;

public sealed record CnbExchangeRateResponseItem(long Amount, string Country, string Currency, string CurrencyCode, int Order, decimal Rate, string ValidFor);