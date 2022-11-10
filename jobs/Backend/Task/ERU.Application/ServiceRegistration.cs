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
		services.AddScoped<ICache, MemoryCache>(sp =>
		{
			CacheSettings? settings = sp.GetRequiredService<IOptions<CacheSettings>>().Value;
			var cache = sp.GetRequiredService<IMemoryCache>();
			return new MemoryCache(cache, settings.AbsoluteExpirationInMinutes, settings.SlidingExpirationInMinutes);
		});
		services.AddScoped<IHttpClient, PrimitiveHttpClient>();
		services.AddScoped<IDataStringParser<IEnumerable<CnbExchangeRateResponse>>, CnbExchangeRateDataParser>();
		services.AddScoped<IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate>>(sp =>
		{
			ConnectorSettings? settings = sp.GetRequiredService<IOptions<ConnectorSettings>>().Value;
			return new CnbExchangeRateMapper(settings.SourceCurrency);
		});
		services.AddScoped<IExchangeRateProvider, ExchangeRateService>();
		services.AddScoped<IDataExtractor, CnbDataExtractor>(sp =>
		{
			ConnectorSettings? settings = sp.GetRequiredService<IOptions<ConnectorSettings>>().Value;
			var httpClient = sp.GetRequiredService<IHttpClient>();
			var cache = sp.GetRequiredService<ICache>();
			var parser = sp.GetRequiredService<IDataStringParser<IEnumerable<CnbExchangeRateResponse>>>();
			return new CnbDataExtractor(httpClient, parser, settings.FileUri.ToList(), cache);
		});
	}
}