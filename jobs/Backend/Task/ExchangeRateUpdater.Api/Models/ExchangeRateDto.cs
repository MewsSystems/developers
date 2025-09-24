namespace ExchangeRateUpdater.Api.Models;

public record ExchangeRateDto(
    string SourceCurrency,
    string TargetCurrency,
    decimal Value,
    DateTime Date);

public record ExchangeRateRequest(
    List<string> Currencies,
    DateTime? Date = null);

public record ExchangeRateResponse(
    List<ExchangeRateDto> Rates,
    DateTime RequestedDate,
    int TotalCount);
    