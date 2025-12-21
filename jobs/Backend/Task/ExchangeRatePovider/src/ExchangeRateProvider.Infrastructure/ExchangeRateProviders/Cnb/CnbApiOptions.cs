namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;

/// <summary>
/// Configuration options for the Czech National Bank (CNB) API integration.
/// </summary>
public record CnbApiOptions
{
    /// <summary>
    /// The configuration section name for CNB API options.
    /// </summary>
    public const string SectionName = "CnbApi";

    /// <summary>
    /// The name of the HTTP client to use for CNB API requests.
    /// </summary>
    public required string HttpClientName { get; init; }

    /// <summary>
    /// The base URL for the CNB API.
    /// </summary>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// The endpoint path for retrieving daily exchange rates from the CNB API.
    /// </summary>
    public required string DailyExchangeRatesEndpoint { get; init; }
}
