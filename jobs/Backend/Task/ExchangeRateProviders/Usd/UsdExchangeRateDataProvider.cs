using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Usd.Config;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace ExchangeRateProviders.Usd
{
	public class UsdExchangeRateDataProvider : IExchangeRateDataProvider
	{
		private readonly ILogger<UsdExchangeRateDataProvider> _logger;

		public UsdExchangeRateDataProvider(
			IFusionCache cache,
			ILogger<UsdExchangeRateDataProvider> logger)
		{
			_logger = logger;
		}

		public string ExchangeRateProviderTargetCurrencyCode => Constants.ExchangeRateProviderCurrencyCode;

		public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken = default)
		{
			var allRates = new List<ExchangeRate>
					{
						new(new Currency("EUR"), new Currency("USD"), 1.18m, DateTime.UtcNow), 
						new(new Currency("JPY"), new Currency("USD"), 0.009m, DateTime.UtcNow), 
						new(new Currency("GBP"), new Currency("USD"), 1.33m, DateTime.UtcNow), 
						new(new Currency("AUD"), new Currency("USD"), 0.74m, DateTime.UtcNow),
						new(new Currency("CAD"), new Currency("USD"), 0.80m, DateTime.UtcNow), 
						new(new Currency("CZK"), new Currency("USD"), 0.044m, DateTime.UtcNow), 
						new(new Currency("CHF"), new Currency("USD"), 1.10m, DateTime.UtcNow),   
						new(new Currency("SEK"), new Currency("USD"), 0.095m, DateTime.UtcNow),  
						new(new Currency("NOK"), new Currency("USD"), 0.093m, DateTime.UtcNow), 
						new(new Currency("DKK"), new Currency("USD"), 0.158m, DateTime.UtcNow),  
						new(new Currency("NZD"), new Currency("USD"), 0.61m, DateTime.UtcNow),   
						new(new Currency("CNY"), new Currency("USD"), 0.14m, DateTime.UtcNow),   
						new(new Currency("INR"), new Currency("USD"), 0.012m, DateTime.UtcNow),
						new(new Currency("BRL"), new Currency("USD"), 0.20m, DateTime.UtcNow),  
						new(new Currency("MXN"), new Currency("USD"), 0.058m, DateTime.UtcNow), 
						new(new Currency("ZAR"), new Currency("USD"), 0.052m, DateTime.UtcNow)
					};

			return await Task.FromResult(allRates.AsEnumerable());
		}
	}
}
