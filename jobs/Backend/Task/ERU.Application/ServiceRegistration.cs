using ERU.Application.DTOs;
using ERU.Application.Interfaces;
using ERU.Application.Services.ExchangeRate;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ERU.Application;

/// <summary>
/// Class for service registration.
/// </summary>
public static class ServiceRegistration
{
  /// <param name="services">Extended app services callback.</param>
  public static void RegisterApplicationLayerServices(this IServiceCollection services)
  {
    services.AddMemoryCache();
    services.AddSingleton(x =>
    {
      CacheSettings? settings = x.GetRequiredService<IOptions<CacheSettings>>().Value;
      var cache = x.GetRequiredService<IMemoryCache>();
      return new MemoryCacheHelper(cache, settings.AbsoluteExpirationInMinutes, settings.SlidingExpirationInMinutes);
    });
    services.AddScoped<IHttpClient, PrimitiveHttpClient>();
    services.AddScoped<IDataStringParser<IEnumerable<CnbExchangeRateResponse>>, CnbExchangeRateDataParser>();
    services.AddScoped<IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate>, CnbExchangeRateMapper>();
    services.AddScoped<IExchangeRateProvider, ExchangeRateService>();
    services.AddScoped<ICnbDataExtractor, CnbDataExtractor>();
  }
}