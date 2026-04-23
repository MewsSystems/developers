namespace ExchangeRateUpdater.Infrastructure.Dto;

internal record ExRateDailyRest(long Amount, string Country, string Currency, string CurrencyCode, int Order, decimal Rate, string ValidFor);