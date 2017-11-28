namespace OpenExchangeRates.Http {
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IOpenExchangeRateClient : IDisposable {
		Task<OpenExchangeRate> GetAsync(IEnumerable<string> currencies);
	}
}