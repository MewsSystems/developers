namespace ExchangeRateUpdater.Core.DTOs;

public record RateInfoDto(
    string ValidFor, 
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    decimal Rate);