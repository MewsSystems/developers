namespace ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;

public record CnbExchangeRatesDailyRate(DateOnly ValidFor, int Order, string Country, string Currency, int Amount, string CurrencyCode, decimal Rate);