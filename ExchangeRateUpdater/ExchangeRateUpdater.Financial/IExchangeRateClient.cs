namespace ExchangeRateUpdater.Financial {
	using System;
	using System.Threading.Tasks;

	public interface IExchangeRateClient : IDisposable {
		Task<TResult> GetAsync<TResult>(string location);
	}
}
