namespace Blockchain {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Blockchain.Http;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;

	public class BlockchainExchangeRateProvider : ExchangeRateProvider {
		private IBlockchainExchangeRateClient _client;

		public BlockchainExchangeRateProvider(IBlockchainExchangeRateClient client, ICurrencyValidator validator)
			: base(validator) {
			_client = Ensure.IsNotNull(client, nameof(client));
		}

		protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies) {
			var temp = await _client.GetAsync();

			var result = temp.Join(currencies, oks => oks.Key, iks => iks.Code, (t, c) => new ExchangeRate(new Currency("BTC"), new Currency(t.Key), t.Value.Last));

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
