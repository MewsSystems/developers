namespace Mews.ExchangeRateProvider.Extensions;

/// <summary>
/// Configuration options for the Czech National Bank exchange rate provider
/// </summary>
public sealed class CzechNationalBankExchangeRateProviderOptions
{
    /// <summary>
    /// The name for this configuration section used in the configuration file
    /// </summary>
    public const string Section = "CzechNationalBankExchangeRateProviderConfiguration";

    /// <summary>
    /// A list of the exchange rate provider configuration objects.
    /// </summary>
    public IEnumerable<CzechNationalBankExchangeRateProviderConfiguration>? ExchangeRateProviders { get; init; }
}

/// <summary>
/// Configuration for a single exchange rate provider for the Czech National Bank data source
/// </summary>
public sealed class CzechNationalBankExchangeRateProviderConfiguration
{
    /// <summary>
    /// The URI to obtain exchange rate details
    /// </summary>
    public Uri? Uri { get; init; }
}