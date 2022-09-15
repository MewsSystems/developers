using ExchangeRateUpdater.Domain.CoreModels;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExchangeRateUpdater.WebApi.Configurations;

/// <summary>
/// Configuration of exception handlers.
/// </summary>
public static class ExceptionConfiguration
{
  /// <summary>
  /// Custom exception handler, adjusted for api responses.
  /// </summary>
  /// <param name="app"></param>
  public static void UseCustomExceptionHandler(this IApplicationBuilder app)
  {
    app.UseExceptionHandler(a => a.Run(async context =>
    {
      context.Response.ContentType = "application/json";
      IExceptionHandlerPathFeature? feature = context.Features.Get<IExceptionHandlerPathFeature>();

      if (feature != null)
      {
        ExceptionResponseObject response = new ExceptionResponseObject(feature.Error);

        string json = JsonConvert.SerializeObject(response,
          new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        await context.Response.WriteAsync(json);
      }
    }));
  }
}