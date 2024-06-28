namespace Mews.ExchangeRateUpdater.Infrastructure.HttpClients.Config;

/// <summary>
/// Settings for API URLs.
/// </summary>
public class ApiUrlsSettings
{
    /// <summary>
    /// Section name.
    /// </summary>
    public const string SectionName = "ApiUrls";

    /// <summary>
    /// CzechNationalBankApi URL.
    /// </summary>
    public string CzechNationalBankApi { get; set; } = null!;
}