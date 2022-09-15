using ExchangeRateUpdater.Domain.CoreModels;

namespace ExchangeRateUpdater.WebApi.Configurations;

/// <summary>
/// Class for cors configuration.
/// </summary>
public static class CorsConfiguration
{
  private static string? _corsPolicyName;
    
  /// <summary>
  /// Register cors policy from application.json settings.
  /// </summary>
  /// <param name="services"></param>
  /// <param name="config"></param>
  /// <exception cref="Exception"></exception>
  public static void RegisterCorsPolicy(this IServiceCollection services, IConfiguration config)
  {
    CorsConfig corsConfig = config.GetSection("CorsConfig").Get<CorsConfig>();
    if (corsConfig == null)
    {
      throw new Exception("Cors not configured.");
    }

    _corsPolicyName = corsConfig.Name;
    services.AddCors(options => options.AddPolicy(corsConfig.Name, builder =>
    {
      if (!string.IsNullOrWhiteSpace(corsConfig.AllowOrigin) && !corsConfig.AllowOrigin.Equals("any", StringComparison.InvariantCultureIgnoreCase))
      {
        builder.WithOrigins(corsConfig.AllowOrigin);
        builder.AllowCredentials();
      }
      else
      {
        builder.AllowAnyOrigin();
      }

      if (!string.IsNullOrWhiteSpace(corsConfig.AllowMethod) && !corsConfig.AllowMethod.Equals("any", StringComparison.InvariantCultureIgnoreCase))
      {
        builder.WithMethods(corsConfig.AllowMethod);
      }
      else
      {
        builder.AllowAnyMethod();
      }

      if (!string.IsNullOrWhiteSpace(corsConfig.AllowHeader) && !corsConfig.AllowHeader.Equals("any", StringComparison.InvariantCultureIgnoreCase))
      {
        builder.WithHeaders(corsConfig.AllowHeader);
      }
      else
      {
        builder.AllowAnyHeader();
      }
    }));
  }

  /// <summary>
  /// Use previously registered cors policy.
  /// </summary>
  /// <param name="app"></param>
  public static void UseCorsPolicy(this IApplicationBuilder app)
  {
    if (string.IsNullOrWhiteSpace(_corsPolicyName))
    {
      throw new Exception("Missing registered cors policy.");
    }
    app.UseCors(_corsPolicyName);
  }
}