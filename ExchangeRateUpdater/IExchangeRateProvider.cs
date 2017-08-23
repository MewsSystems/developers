using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// Provides currency exchange rates
	/// </summary>
	public interface IExchangeRateProvider
	{
		/// <summary>
		/// Gets list of currency's exchange rates
		/// </summary>
		/// <param name="currencies">Currencies which need to get exchange rates for</param>
		/// <returns>The list of currency's exchange rates</returns>
		/// <exception cref="ExchangeRateProviderException">ExchangeRateProviderException</exception>
		IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
	}
}
