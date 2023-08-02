namespace CnbApiClient.Model;

public record CnbExchangeRates(
    IEnumerable<CnbExchangeRate> Rates
);