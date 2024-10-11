using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Provider
{
	public interface IExchangeRateProvider
	{
			Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
	}
}
