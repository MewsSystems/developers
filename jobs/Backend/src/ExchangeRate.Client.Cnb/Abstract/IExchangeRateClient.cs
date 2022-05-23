using ExchangeRate.Client.Cnb.Models.Txt;
using ExchangeRate.Client.Cnb.Models.Xml;

namespace ExchangeRate.Client.Cnb.Abstract
{
	/// <summary>
	///     Client for getting Exchange Rate from CNB
	/// </summary>
	public interface IExchangeRateClient
	{
		/// <summary>
		///     Gets the Exchange Rate data from txt api source
		/// </summary>
		/// <returns><see cref="TxtExchangeRate"/></returns>
		Task<List<TxtExchangeRate>> GetExchangeRatesTxtAsync();

		/// <summary>
		///     Gets the Exchange Rate data from xml api source
		/// </summary>
		/// <returns><see cref="XmlExchangeRate"/></returns>
		Task<XmlExchangeRate> GetExchangeRatesXmlAsync();
	}
}
