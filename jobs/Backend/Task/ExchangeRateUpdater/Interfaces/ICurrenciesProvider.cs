using System.Collections.Generic;
using ExchangeEntities;

namespace ExchangeRateUpdater.Interfaces
{
	public interface ICurrenciesProvider
	{
		IEnumerable<Currency> GetCurrenciesFromConfig();
	}
}

