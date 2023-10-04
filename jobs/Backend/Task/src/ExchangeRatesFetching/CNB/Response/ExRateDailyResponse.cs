namespace ExchangeRatesFetching.CNB.Response;

public record ExRateDailyResponse(List<ExRateDailyRest> Rates);

public record ExRateDailyRest(long Amount, string Country, string Currency, string CurrencyCode, int Order, decimal Rate, DateTime ValidFor);
