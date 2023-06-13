using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
	public class CnbClient : ICnbClient
	{
		private readonly HttpClient httpClient;
		private readonly ILogger<CnbClient> logger;
		private readonly string url;

		public CnbClient(HttpClient httpClient, ILogger<CnbClient> logger, IOptions<CnbDailyRatesOptions> options)
		{
			this.httpClient = httpClient;
			this.logger = logger;
			url = options.Value.Url!;
		}

		public async Task<string> GetRatesAsync(DateOnly? date = null)
		{
			try
			{
				var fullUrl = date.HasValue
					? $"{url}?date={date.Value:dd.MM.yyyy}"
					: url;
				return await httpClient.GetStringAsync(fullUrl);
			}
			catch (Exception e)
			{
				// TODO Handle other exceptions
				logger.LogError(e, "Could not get exchange rates");
				throw;
			}
		}
	}
}