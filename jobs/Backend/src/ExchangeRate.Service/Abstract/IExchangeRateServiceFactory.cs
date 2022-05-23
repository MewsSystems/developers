using ExchangeRate.Client.Cnb;

namespace ExchangeRate.Service.Abstract
{
	public interface IExchangeRateServiceFactory
	{
		/// <summary>
		/// Get concrete exchange rate implementation by api type
		/// </summary>
		/// <param name="apiType">api type</param>
		/// <returns>Concrete implementation for exchange rate</returns>
		IConcreteExchangeRateService GetExchangeRateService(CnbConstants.ApiType apiType);
	}
}
