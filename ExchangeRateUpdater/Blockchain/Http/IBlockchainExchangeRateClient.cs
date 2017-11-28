namespace Blockchain.Http {
	using System;
	using System.Threading.Tasks;

	public interface IBlockchainExchangeRateClient : IDisposable {
		Task<BlockchainExchangeRateDictionary> GetAsync();
	}
}