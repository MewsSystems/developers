namespace OpenExchangeRates {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;
	using OpenExchangeRates.Http;

	public sealed class OpenExchangeRateProvider : ExchangeRateProvider, IDisposable {
		private IOpenExchangeRateClient _client;

		public OpenExchangeRateProvider(IOpenExchangeRateClient client, ICurrencyValidator validator)
			: base(validator) {
			_client = Ensure.IsNotNull(client);
		}

		protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies) {
			var temp = await _client.GetAsync(currencies.Select(c => c.Code));

			var result = temp.Rates
				.Where(r => temp.Base != r.Key)
				.Select(r => new ExchangeRate(new Currency(temp.Base), new Currency(r.Key), r.Value));

			return result;
		}

		#region IDisposable implementation
		private bool isDisposed = false;

		void Dispose(bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					_client.Dispose();
					_client = null;
				}

				isDisposed = true;
			}
		}

		public override void Dispose() {
			Dispose(true);
		}
		#endregion
	}
}