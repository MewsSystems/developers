namespace OpenExchangeRates.Http.Configuration {
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Text;
	using ExchangeRateUpdater.Configuration;

	public interface IOpenExchangeRateHttpClientConfiguration : IConfiguration<HttpClient> {
	}
}
