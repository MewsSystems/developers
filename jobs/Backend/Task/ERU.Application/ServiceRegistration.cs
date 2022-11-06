using ERU.Application.DTOs;
using ERU.Application.Interfaces;
using ERU.Application.Services.ExchangeRate;
using Microsoft.Extensions.DependencyInjection;

namespace ERU.Application;

/// <summary>
/// Class for service registration.
/// </summary>
public static class ServiceRegistration
{
  /// <param name="services">Extended app services callback.</param>
  public static void RegisterApplicationLayerServices(this IServiceCollection services)
  {
    services.AddScoped<IHttpClient, PrimitiveHttpClient>();
    services.AddScoped<IDataStringParser<IEnumerable<CnbExchangeRateResult>>, CnbExchangeRateDataStringParser>();
    services.AddScoped<IContractObjectMapper<CnbExchangeRateResult, Domain.ExchangeRate>, CnbExchangeRateMapper>();
  }
}