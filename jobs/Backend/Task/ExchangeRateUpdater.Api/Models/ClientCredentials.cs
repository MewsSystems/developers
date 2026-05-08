namespace ExchangeRateUpdater.Api.Models;

public record ClientCredentials
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
} 