using System.Globalization;
using ExchangeRateUpdater.Localization;
using Microsoft.AspNetCore.Localization;

namespace ExchangeRateUpdater.WebApi.Configurations;

/// <summary>
/// Configuration file for localization.
/// </summary>
public static class LocalizationConfiguration
{
  /// <summary>
  /// Register supported localization cultures.
  /// </summary>
  /// <param name="services"></param>
  public static void RegisterLocalization(this IServiceCollection services)
  {
    services.AddJsonLocalization();

    List<string> supportedLanguages = new List<string>
    {
      "en"
    };

    List<CultureInfo> supportedCultures = supportedLanguages.Select(lang => new CultureInfo(lang)).ToList();
    services.Configure<RequestLocalizationOptions>(options =>
    {
      options.DefaultRequestCulture = new RequestCulture("en", "en");
      // Formatting numbers, dates, etc.
      options.SupportedCultures = supportedCultures;
      // UI strings that we have localized.
      options.SupportedUICultures = supportedCultures;
    });
    services.AddSingleton<Localizer>();
  }

  /// <summary>
  /// Include localization packages.
  /// </summary>
  /// <param name="app"></param>
  public static void UseLocalization(this IApplicationBuilder app)
  {
    app.UseRequestLocalization();
  }
}