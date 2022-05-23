using System.Net;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;

namespace Framework.BaseHttpClient
{
	/// <summary>
	/// Base Http client
	/// </summary>
	public abstract class BaseHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<BaseHttpClient> _logger;

		protected BaseHttpClient(HttpClient httpClient, ILogger<BaseHttpClient> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		/// <summary>
		/// Get async
		/// </summary>
		/// <param name="url">url</param>
		/// <returns>string response</returns>
		/// <exception cref="ArgumentNullException"></exception>
		protected async Task<string> GetAsync(string? url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException(url, "Parameter: URL cannot be null or empty.");
			}

			HttpResponseMessage response = await FetchDataAsync(url);
			return await GetResponseContent(response);
		}

		#region Private members
		private async Task<HttpResponseMessage> FetchDataAsync(string url)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync(url);
				if (IsResponseInvalid(response))
				{
					throw new HttpRequestException($"Http request error: {response.StatusCode}");
				}

				return response;
			}
			catch (Exception e)
			{
				_logger.LogCritical(e, "Http call failed");
				throw;
			}
		}

		/// <summary>
		/// Returns response string content
		/// </summary>
		/// <param name="response">Http response</param>
		/// <returns>string response</returns>
		/// <exception cref="EmptyResultSetException"></exception>
		private static async Task<string> GetResponseContent(HttpResponseMessage response)
		{
			if (response is null)
			{
				throw new EmptyResultSetException("Empty response");
			}

			string responseContent = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrWhiteSpace(responseContent))
			{
				throw new EmptyResultSetException("No content available for CNB exchange rate request");
			}

			return responseContent;
		}

		/// <summary>
		/// HTTP response code validation
		/// </summary>
		/// <param name="response">Http response</param>
		/// <returns>true - if response code is invalid
		///			 false - if response code is valid</returns>
		private static bool IsResponseInvalid(HttpResponseMessage response)
			=> response.StatusCode is HttpStatusCode.InternalServerError
				or HttpStatusCode.BadGateway
				or HttpStatusCode.BadRequest
				or HttpStatusCode.GatewayTimeout
				or HttpStatusCode.Forbidden
				or HttpStatusCode.NotFound
				or HttpStatusCode.NotImplemented;
		#endregion
	}
}
