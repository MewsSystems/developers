namespace ExchangeRateUpdater.Api.Models;

public record ExchangeRateDto(
    string SourceCurrency,
    string TargetCurrency,
    decimal Value,
    DateTime Date);

public record ExchangeRateResponseDto(
    List<ExchangeRateDto> Rates,
    DateTime RequestedDate,
    int TotalCount);
    
