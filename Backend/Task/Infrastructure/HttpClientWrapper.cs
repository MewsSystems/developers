using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
	/*
	 * Although HttpClient does indirectly implement the IDisposable interface, 
	 * the standard usage of HttpClient is not to dispose of it after every request. 
	 * The HttpClient object is intended to live for as long as your application needs to make HTTP requests. 
	 * Having an object exist across multiple requests enables a place for setting DefaultRequestHeaders and prevents you 
	 * from having to re-specify things like CredentialCache and CookieContainer on every request as was necessary with HttpWebRequest.
	 */

	public static class HttpClientWrapper
	{
		static HttpClient client;

		static HttpClientWrapper()
		{
			// client configuration - this and any other specifications can be taken from configuration
			client = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(30) 
			};
		}

		public static async Task<string> GetStringAsync(string url)
		{
			string result = null;

			try
			{
				HttpResponseMessage response = await client.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					result = await response.Content.ReadAsStringAsync();
				}
			}
			catch (System.Exception)
			{
				// logging...
			}

			return result;
		}
	}
}
