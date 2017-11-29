namespace Blockchain {
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Blockchain.Http;
	using ExchangeRateUpdater.Financial;

	public class BlockchainExhangeRateProviderOptions : ExchangeRateProviderOptions, IBlockchainExchangeRateProviderOptions {
		public BlockchainExhangeRateProviderOptions(IBlockchainExchangeRateClient client, ICurrencyValidator validator) : base(client, validator) { }
	}
}
