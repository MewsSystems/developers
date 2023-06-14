using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
	public class CnbClient : ICnbClient
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly ILogger<CnbClient> logger;
		private readonly string url;

		public CnbClient(IHttpClientFactory httpClientFactory, ILogger<CnbClient> logger, IOptions<CnbDailyRatesOptions> options)
		{
			this.httpClientFactory = httpClientFactory;
			this.logger = logger;
			url = options.Value.Url!;
		}

		public async Task<string> GetRatesAsync(DateOnly? date = null)
		{
			var fullUrl = date.HasValue
				? $"{url}?date={date.Value:dd.MM.yyyy}"
				: url;
			try
			{
				return await httpClientFactory.CreateClient().GetStringAsync(fullUrl);
			}
			catch (Exception e)
			{
				logger.LogError(e, "Could not get exchange rates from {Url}", fullUrl);
				throw;
			}
		}
	}
}