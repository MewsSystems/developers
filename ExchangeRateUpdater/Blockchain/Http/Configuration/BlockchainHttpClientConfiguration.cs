using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ExchangeRateUpdater.Diagnostics;

namespace Blockchain.Http.Configuration
{
	public class BlockchainHttpClientConfiguration : IBlockchainHttpClientConfiguration {
		public HttpClient Configure(HttpClient target) {
			var httpClient = Ensure.IsNotNull(target);

			httpClient.BaseAddress = new Uri("https://blockchain.info/ticker");
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

			return httpClient;
		}
	}
}
