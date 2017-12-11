namespace Blockchain {
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Blockchain.Http;
	using ExchangeRateUpdater.Financial;

	public class BlockchainExchangeRateProvider : ExchangeRateProvider<IBlockchainExchangeRateProviderOptions> {
		private static readonly string requestUriString = "?cors=true";

		public BlockchainExchangeRateProvider(IBlockchainExchangeRateProviderOptions options)
			: base(options) {
		}

		protected override string CreateRequestUriString(IEnumerable<Currency> currencies) {
			return requestUriString;
		}

		protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies) {
			var temp = await Client.GetAsync<BlockchainExchangeRateDictionary>(requestUriString);

			var result = temp.Join(currencies, oks => oks.Key, iks => iks.Code, (t, c) => new ExchangeRate(new Currency("BTC"), new Currency(t.Key), t.Value.Last));

			return result;
		}
	}
}
