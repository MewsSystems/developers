namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;

public sealed record GetExratesDailyRequest(DateOnly Date);
public sealed record GetExratesDailyResponse(IEnumerable<CurrencyExchangeRateDto> Rates);
public sealed record CurrencyExchangeRateDto(string SourceCurrency, string TargetCurrency, decimal Amount, decimal Rate);