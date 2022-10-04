using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRates.Contracts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRates.Clients
{
	public class CnbClient : ICnbClient
	{
		private readonly IHttpClientFactory httpFactory;
		private readonly ILogger<CnbClient> logger;
		private readonly AppSettings options;		

		public CnbClient(
			IHttpClientFactory httpFactory,
			ILogger<CnbClient> logger,
			IOptions<AppSettings> options) 
		{
			this.httpFactory = httpFactory;
			this.logger = logger;
			this.options = options.Value;
		}

		public async Task<string> GetExchangeRatesAsync(DateOnly? date)
		{
			var request = BuildRequest(date);
			var client = httpFactory.CreateClient();			
			var response = await client.SendAsync(request);			

			if (response.IsSuccessStatusCode)
			{
				logger.LogInformation($"Exchange rates from CNB have been succesfully downloaded.");
				return await response.Content.ReadAsStringAsync();
			}
			else
			{
				logger.LogWarning($"Exchange rates from CNB could not be downloaded.");
				return string.Empty;
			}
		}

		private HttpRequestMessage BuildRequest(DateOnly? date) 
		{
			var queryParameters = new Dictionary<string, string>();
			if (date.HasValue)
			{
				queryParameters.Add("date", date.Value.ToString("dd.MM.yyyy"));
			}

			var uri = QueryHelpers.AddQueryString(options.Urls.CnbUrl, queryParameters);
			return new HttpRequestMessage(
				HttpMethod.Get,
				uri);
		}
	}
}
