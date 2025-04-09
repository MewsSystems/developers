namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Client;

internal class ApiClientOptions
{
    public static string SectionName => nameof(ApiClientOptions);

    public required string Host { get; set; }

    public required string Scheme { get; set; }

    public required string DailyRatesEndpoint { get; set; }
}