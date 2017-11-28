namespace CeskaNarodniBanka.Http.Configuration {
	using System;
	using System.Net.Http;
	using ExchangeRateUpdater.Diagnostics;

	public class CnbHttpClientConfiguration : ICnbHttpClientConfiguration {
		public HttpClient Configure(HttpClient target) {
			var httpClient = Ensure.IsNotNull(target);

			httpClient.BaseAddress = new Uri("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/");
			httpClient.DefaultRequestHeaders.Add("Accept", "text/xml");

			return httpClient;
		}
	}
}
