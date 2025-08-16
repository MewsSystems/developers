namespace ExchangeRateUpdater.Settings;

public class ApplicationSettings
{
    /// <summary>
    ///     The name used to bind the configuration section
    /// </summary>
    public const string ConfigurationName = "Application";

    /// <summary>
    ///     The base currency code to use as a target currency
    /// </summary>
    [Required]
    public string BaseCurrencyCode { get; set; } = default!;
}