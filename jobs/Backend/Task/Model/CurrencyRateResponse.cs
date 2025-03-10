namespace CurrencyRateUpdater;

public record CurrencyRatesResponse(List<CurrencyRateResponse> Rates);
public record CurrencyRateResponse(string Currency, string CurrencyCode, decimal Rate);