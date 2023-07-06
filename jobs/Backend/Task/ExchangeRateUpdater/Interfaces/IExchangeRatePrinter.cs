using System.Collections.Generic;
using ExchangeEntities;

namespace ExchangeRateUpdater.Interfaces
{
	public interface IExchangeRatePrinter
	{
		/// <summary>
		/// Prints the <see cref="ExchangeRate"/> list to the Console.
		/// </summary>
		/// <param name="exchangeRates"><see cref="ExchangeRate"/> list to be printed.</param>
		void Print(IEnumerable<ExchangeRate> exchangeRates);
	}
}

