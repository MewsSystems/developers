namespace ExchangeRateUpdater.Domain.CoreModels;

/// <summary>
/// Class holds data from the "CorsConfig" section of the appsettings.*.json file.
/// </summary>
public class CorsConfig
{
  /// <summary>
  /// Allow method setting.
  /// </summary>
  public string AllowMethod { get; set; }
  /// <summary>
  /// Allow origin setting.
  /// </summary>
  public string AllowOrigin { get; set; }
  /// <summary>
  /// Allow header setting.
  /// </summary>
  public string AllowHeader { get; set; }
  /// <summary>
  /// Name setting.
  /// </summary>
  public string Name { get; set; }
}