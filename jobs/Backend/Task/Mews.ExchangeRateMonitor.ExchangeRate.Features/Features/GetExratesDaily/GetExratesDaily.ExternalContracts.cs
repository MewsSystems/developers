namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;

public sealed record CnbApiDailyRatesResponseDto(IEnumerable<CnbApiDailyRateDto> Rates);
public sealed record CnbApiDailyRateDto(
    decimal Amount,
    string Country,
    string Currency,
    string CurrencyCode,
    int Order,
    decimal Rate,
    DateTime ValidFor);

