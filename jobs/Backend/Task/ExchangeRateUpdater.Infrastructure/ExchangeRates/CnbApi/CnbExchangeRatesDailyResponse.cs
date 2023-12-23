namespace ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;

public record CnbExchangeRatesDailyResponse(List<CnbExchangeRatesDailyRate> Rates) { }