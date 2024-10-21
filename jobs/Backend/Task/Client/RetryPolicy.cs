using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
	public class RetryPolicy : IRetryPolicy
	{
		readonly ILogger _logger;
        public RetryPolicy(ILogger<RetryPolicy> logger)
        {
            _logger = logger;
        }
        public async Task<HttpResponseMessage> ExecuteGetRequestWithRetry(HttpClient client, string relativeUri, int maxRetries, int requestInterval)
		{
			HttpResponseMessage response = null;

			for (int attempt = 1; attempt <= maxRetries; attempt++)
			{

				try
				{
					if (attempt > 1)
					{
						Thread.Sleep(TimeSpan.FromSeconds(requestInterval));
					}

					response = await client.GetAsync(relativeUri);
				}
				catch (Exception ex)
				{
					LogClientException(client.BaseAddress.ToString(), ex, attempt);
					continue;
				}

				if (response.IsSuccessStatusCode)
				{
					LogSuccessfullRequest(client.BaseAddress.ToString());
					return response;
				}
				else
				{
					LogFailedRequest(client.BaseAddress.ToString(), response.StatusCode.ToString(), attempt);
				}
			}
			
			return response;
		}

		void LogClientException(string uri, Exception ex, int attemptNumber) => _logger.LogError(
						$"Exception on GET request to {uri} on {DateTime.Now} number {attemptNumber}. Exception thrown: {ex.GetType()}: {ex.Message}");

		void LogSuccessfullRequest(string uri) => _logger.LogInformation(
						$"Successful GET request to {uri} on {DateTime.Now}.");

		void LogFailedRequest(string uri, string statusCode, int attemptNumber) => _logger.LogWarning(
						$"Failed GET request to {uri} on {DateTime.Now} number {attemptNumber}. Response: Statuc code {statusCode}.");
	}
}
