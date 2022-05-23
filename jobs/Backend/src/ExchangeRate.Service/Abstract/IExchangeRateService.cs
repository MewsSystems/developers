using ExchangeRate.Client.Cnb;

namespace ExchangeRate.Service.Abstract
{
	/// <summary>
	///     Exchange rate service for CNB APIs
	/// </summary>
	public interface IExchangeRateService
	{
		/// <summary>
		/// Get exchange rates by api type
		/// </summary>
		/// <param name="apiType">api type</param>
		/// <returns>list string exchange rates</returns>
		Task<List<string>?> GetExchangeRates(CnbConstants.ApiType apiType);
	}
}
