using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class HttpClientWrapper : IHttpClientWrapper
    {

		private readonly HttpClient HttpClient;

		public HttpClientWrapper()
		{
			HttpClient = new HttpClient();
		}

		public void Dispose()
        {
			HttpClient?.Dispose();
		}

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
			return await HttpClient.GetAsync(url);
		}
    }
}
