using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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

		public async Task<string> GetExchangeRatesAsync(DateOnly? date, CancellationToken token = default)
		{			
			
			HttpResponseMessage response;
			try 
			{
				var client = BuildClient();
				var request = BuildRequest(date);
				response = await client.SendAsync(request, token);
			}
			catch (OperationCanceledException ex)
			{
				logger.LogError(ex, "Exchange rates request has been cancelled.");
				throw;
			}
			catch (Exception ex) 
			{
				logger.LogError(ex, "Error while executing the exchange rate request.");
				throw;
			}
												
			if (response.IsSuccessStatusCode)
			{
				logger.LogInformation("Exchange rates from CNB have been succesfully downloaded.");
				return await response.Content.ReadAsStringAsync(token);
			}
			else
			{
				logger.LogWarning("Exchange rates from CNB could not be downloaded.");
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

		private HttpClient BuildClient() 
		{
			var client = httpFactory.CreateClient();
				
			client.Timeout = TimeSpan.FromSeconds(30);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new("text/plain"));			

			return client;
		}
	}
}
