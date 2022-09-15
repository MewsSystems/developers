using ExchangeRateUpdater.Domain.CoreModels;

namespace ExchangeRateUpdater.WebApi.Configurations;

/// <summary>
/// Class for exchange rate provider configuration.
/// </summary>
public static class ExchangeRateProviderConfiguration
{
  /// <summary>
  /// Register exchange rates provider from application.json settings.
  /// </summary>
  /// <param name="services"></param>
  /// <param name="config"></param>
  public static void RegisterExchangeRateProvider(this IServiceCollection services, IConfiguration config)
  {
    ExchangeRateProviderSettings providerSettings = config.GetSection("ExchangeRateProvider").Get<ExchangeRateProviderSettings>();
    if (providerSettings == null)
    {
      throw new Exception("Exchange rate provider not configured.");
    }
    
    services.AddSingleton(providerSettings);
  }
}