namespace ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;

public sealed record CnbExRateDaily(
    string ValidFor,
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    decimal Rate);
