namespace OpenExchangeRates.Http {
	using System.Net.Http;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial.Http;

	public class OpenExchangeRateClient : HttpExchangeRateClient, IOpenExchangeRateClient {
		public OpenExchangeRateClient(IOpenExchangeRateClientOptions options)
			: base(options) { }

		protected override async Task<TResult> ReadContentAsync<TResult>(HttpContent httpContent) {
			var result = await Ensure.IsNotNull(httpContent).ReadAsAsync<TResult>();

			return result;
		}
	}
}
