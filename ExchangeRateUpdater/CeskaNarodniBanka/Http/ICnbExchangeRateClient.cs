namespace CeskaNarodniBanka.Http {
	using System;
	using System.Threading.Tasks;

	public interface ICnbExchangeRateClient : IDisposable {
		Task<CnbExchangeRateRoot> GetAsync();
	}
}