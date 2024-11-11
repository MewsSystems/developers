using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		private readonly IExchangeRateApi _exchangeRateApi;
		private readonly ICache<ExchangeRateDitributor> _exchangeRateCache = null;
		private readonly TimeSpan? _cacheDuration = null;
		private readonly bool _useCache = false;
		private const string CZECH_EXRATE_CACHE_KEY = "czech-exchange-rate";
		
		
		public ExchangeRateProvider(IExchangeRateApi exchageRateApi)
		{
			ArgumentNullException.ThrowIfNull(exchageRateApi);
			
			_exchangeRateApi = exchageRateApi;
		}
		
		public ExchangeRateProvider(IExchangeRateApi exchageRateApi, ICache<ExchangeRateDitributor> exchangeRateCache, TimeSpan cacheDuration): this(exchageRateApi)
		{
			ArgumentNullException.ThrowIfNull(exchangeRateCache);
			
			_exchangeRateCache = exchangeRateCache;
			_cacheDuration = cacheDuration;
			_useCache = true;
		}

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
		{
			ArgumentNullException.ThrowIfNull(currencies);

			ExchangeRateDitributor exchangeRateDistributor = await GetExchangeRates();
			List<ExchangeRate> rates = new();
			
			foreach(Currency c in currencies)
			{
				if(exchangeRateDistributor.TryGet(c, out ExchangeRate rate))
				{
					rates.Add(rate);
				}
			}

			return rates;
		}

		private async Task<ExchangeRateDitributor> GetExchangeRates()
		{
			if(_useCache && _exchangeRateCache.TryGet(CZECH_EXRATE_CACHE_KEY, out ExchangeRateDitributor exchangeRateDistributor))
				return exchangeRateDistributor;
			
			exchangeRateDistributor = await _exchangeRateApi.GetLastExchangeRateAsync();
			
			if(_useCache)
				_exchangeRateCache.Add(CZECH_EXRATE_CACHE_KEY, exchangeRateDistributor, DateTime.UtcNow.Add(_cacheDuration.Value));
			
			return exchangeRateDistributor;
		}
		
	}
}
