namespace ExchangeRateUpdater.Settings;

public class ExternalBankApiSettings
{
    /// <summary>
    ///     The name used to bind the configuration section
    /// </summary>
    public const string ConfigurationName = "ExternalBankApi";

    /// <summary>
    ///     The base URI to use for all requests related to the external bank API.
    ///     Usually set to <c>https://api.cnb.cz/cnbapi/</c>
    /// </summary>
    [Required]
    [Url]
    public string BaseUri { get; set; } = default!;
}