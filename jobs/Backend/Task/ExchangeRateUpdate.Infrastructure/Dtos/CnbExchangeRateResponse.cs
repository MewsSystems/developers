namespace ExchangeRateUpdater.Infrastructure.Dtos;

public sealed record CnbExchangeRateResponse(IEnumerable<CnbExchangeRateResponseDto> Rates);
