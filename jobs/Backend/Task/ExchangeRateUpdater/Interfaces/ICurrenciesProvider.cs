using System.Collections.Generic;
using ExchangeEntities;

namespace ExchangeRateUpdater.Interfaces
{
	public interface ICurrenciesProvider
	{
        /// <summary>
        /// Gets the configuration parameter `Currencies` and creates a list of <see cref="Currency"/>.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}">IEnumerable&lt;Currency&gt;</see> with all the currencies present in the configuration.</returns>
        IEnumerable<Currency> GetCurrenciesFromConfig();
	}
}

