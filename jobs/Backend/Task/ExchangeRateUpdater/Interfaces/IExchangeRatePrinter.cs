using System.Collections.Generic;
using ExchangeEntities;

namespace ExchangeRateUpdater.Interfaces
{
	public interface IExchangeRatePrinter
	{
		void Print(IEnumerable<ExchangeRate> exchangeRates);
	}
}

