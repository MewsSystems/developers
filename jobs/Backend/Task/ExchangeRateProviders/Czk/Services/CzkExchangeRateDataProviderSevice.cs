using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Czk.Clients;
using ExchangeRateProviders.Czk.Config;
using ExchangeRateProviders.Czk.Mappers;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace ExchangeRateProviders.Czk.Services
{
	public class CzkExchangeRateDataProviderSevice : IExchangeRateDataProvider
	{
		private const string CacheKey = "CnbDailyRates";

		private readonly IFusionCache _cache;
		private readonly ICzkCnbApiClient _apiClient;
		private readonly ILogger<CzkExchangeRateDataProviderSevice> _logger;

		public CzkExchangeRateDataProviderSevice(
			IFusionCache cache,
			ICzkCnbApiClient apiClient,
			ILogger<CzkExchangeRateDataProviderSevice> logger)
		{
			_cache = cache;
			_apiClient = apiClient;
			_logger = logger;
		}

		public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken = default)
		{
			var cacheOptions = CnbCacheStrategy.GetCacheOptionsBasedOnPragueTime();
			_logger.LogDebug("Using cache duration: {Duration} minutes", cacheOptions.Duration.TotalMinutes);

			return await _cache.GetOrSetAsync(CacheKey, async _ =>
			{
				_logger.LogInformation("Cache miss for CNB daily rates. Fetching and mapping.");
				var raw = await _apiClient.GetDailyRatesRawAsync(cancellationToken).ConfigureAwait(false);
				var mapped = raw.MapToExchangeRates();
				_logger.LogInformation("Mapped {Count} CNB exchange rates (target currency {TargetCurrency}).", mapped.Count(), Constants.ExchangeRateProviderCurrencyCode);
				return (IEnumerable<ExchangeRate>)mapped;
			}, cacheOptions);
		}
	}
}
