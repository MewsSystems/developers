namespace ExchangeRateUpdater.Domain.CoreModels;

/// <summary>
/// Class holds data from the "ExchangeRateProvider" section of the appsettings.*.json file.
/// </summary>
public class ExchangeRateProviderSettings
{
  /// <summary>
  /// Name setting.
  /// </summary>
  public string Name { get; set; }
  /// <summary>
  /// The url of an exchange rates source.
  /// </summary>
  public string ExchangeRatesSourceUrl { get; set; }
  /// <summary>
  /// Default currency code.
  /// </summary>
  public string DefaultCurrencyCode { get; set; }
  /// <summary>
  /// Culture info string.
  /// </summary>
  public string CultureInfo { get; set; }
}