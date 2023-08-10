namespace ExchangeRateUpdater.Dto;

public record ExchangeRateBo(string CurrencyCode, decimal Rate, ushort Amount){ }