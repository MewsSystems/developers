namespace ExchangeRateUpdater.Api.Presentation.Contracts.Responses;

public sealed record ExchangeRateResponse(
    string SourceCurrency,
    string TargetCurrency,
    decimal Rate,
    int Amount,
    string ValidFor);
