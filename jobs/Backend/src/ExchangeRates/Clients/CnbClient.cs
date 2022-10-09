using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRates.Contracts;
using ExchangeRates.Parsers;
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
				ValidateInputs();
				var request = BuildRequest(options.Urls.CnbUrl, date);
				var client = BuildClient();				
				response = await client.SendAsync(request, token);
			}
			catch (OperationCanceledException ex)
			{
				logger.LogError(ex, $"[{nameof(CnbClient)}] Exchange rate request has been cancelled.");
				throw;
			}
			catch (Exception ex) 
			{
				logger.LogError(ex, $"[{nameof(CnbClient)}] Error while executing the exchange rate request.");
				throw;
			}
												
			if (response.IsSuccessStatusCode)
			{
				logger.LogInformation($"[{nameof(CnbClient)}] Exchange rates from CNB have been succesfully downloaded.");
				return await response.Content.ReadAsStringAsync(token);
			}
			else
			{
				logger.LogWarning($"[{nameof(CnbClient)}] Exchange rates from CNB could not be downloaded with the status '{response.StatusCode}' ({response.ReasonPhrase}).");
				return string.Empty;
			}
		}

		private HttpRequestMessage BuildRequest(string url, DateOnly? date) 
		{
			var queryParameters = new Dictionary<string, string>();
			if (date.HasValue)
			{
				queryParameters.Add("date", date.Value.ToString("dd.MM.yyyy"));
			}			

			var uri = QueryHelpers.AddQueryString(url, queryParameters);
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

		private void ValidateInputs() 
		{
			// Configured URL check
			if (!Uri.TryCreate(options.Urls.CnbUrl, UriKind.Absolute, out Uri cnbUrl))
			{
				var message = $"[{nameof(CnbClient)}] Provided URL is not in correct format.";
				logger.LogError(message);
				throw new ArgumentException(message);
			}
		}
	}
}
