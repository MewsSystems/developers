namespace ExchangeRateUpdater.Infrastructure.CNB;

public record CnbConfiguration
{
    public required string BaseAddress { get; init; }
    public required int CacheTTLInSeconds { get; init; }
    public required string CacheKeyBase { get; init; }
    public required string Language { get; init; }
}