using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Models;
using ExchangeRate.Service.Abstract;
using Framework.Caching.Abstract;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Service.Service
{
	public class ExchangeRateService : IExchangeRateService
	{

		private readonly IExchangeRateServiceFactory _exchangeRateServiceFactory;
		private readonly ILogger<ExchangeRateService> _logger;
		private readonly ICache _cache;
		private readonly CnbClientConfiguration _cnbConfiguration;

		public ExchangeRateService(IExchangeRateServiceFactory exchangeRateServiceFactory, ICache cache, ILogger<ExchangeRateService> logger, IOptions<CnbClientConfiguration> cnbConfiguration)
		{
			_exchangeRateServiceFactory = exchangeRateServiceFactory;
			_cache = cache;
			_logger = logger;
			_cnbConfiguration = cnbConfiguration.Value;
		}

		public async Task<List<string>?> GetExchangeRates(CnbConstants.ApiType apiType)
		{
			List<string>? result = _cache.Get<List<string>>(CnbConstants.CacheKeys.CacheKeyCnb);
			if (result?.Count > 0)
			{
				_logger.LogDebug("Getting data from cache");
				return result;
			}

			result = await _exchangeRateServiceFactory.GetExchangeRateService(apiType).GetExchangeRates(CnbConstants.BaseCurrency);

			if (result == null || result.Count == 0)
			{
				throw new EmptyResultSetException("Empty exchange rate data");
			}

			_cache.Set(CnbConstants.CacheKeys.CacheKeyCnb, result, _cnbConfiguration.CacheTtl);

			return result;
		}
	}
}
