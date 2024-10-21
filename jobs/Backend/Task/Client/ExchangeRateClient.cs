using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
	public class ExchangeRateClient : IExchangeRateClient
	{
		readonly IHttpClientFactory _clientFactory;
		readonly ILogger<ExchangeRateClient> _logger;
		readonly IRetryPolicy _retryPolicy;

		public ExchangeRateClient(IHttpClientFactory clientFactory, ILogger<ExchangeRateClient> logger, IRetryPolicy retryPolicy)
        {
            _clientFactory = clientFactory;
			_logger = logger;
			_retryPolicy = retryPolicy;
        }
        public async Task<IEnumerable<ExchangeRateEntity>> GetExchangeRateEntitiesAsync()
		{
			var entities = new List<ExchangeRateEntity>();
			var client = _clientFactory.CreateClient(Configuration.ExchangeRatesHttpClient);

			var response = await _retryPolicy.ExecuteGetRequestWithRetry(client, string.Empty, Configuration.MaxRetries, Configuration.RequestInterval);

			if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return entities;
			}
			try
			{
				var dto = await response.Content.ReadFromJsonAsync<ExchangeRatesDto>();
				entities = dto?.ExchangeRates;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error while parsing content from Http response: {ex.GetType()} : {ex.Message}");
				return new List<ExchangeRateEntity>();
			}
			
			return entities;
		}
	}
}