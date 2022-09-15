using ExchangeRateUpdater.Service;

namespace ExchangeRateUpdater.WebApi.Configurations;

/// <summary>
/// Class for service registration.
/// </summary>
public static class ServiceRegistration
{
  /// <summary>
  /// Register your services and interfaces here.
  /// Use appropriate service scopes (transient, scoped, singleton), preferably use scoped.
  /// Read more at: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#service-lifetimes
  /// Warning: register services in alphabetic order!
  /// </summary>
  /// <param name="services">Extended app services callback.</param>
  public static void RegisterLocalServices(this IServiceCollection services)
  {
    services.AddScoped<ExchangeRateProviderCzechNationalBankService>();
  }
}