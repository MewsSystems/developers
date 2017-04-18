using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.BankConnection
{
	interface IExchangeDataProvider
	{
		/// <summary>
		/// Get exchange rate for currency pair
		/// </summary>
		/// <param name="code1">First currency code</param>
		/// <param name="code2">Second currency code</param>
		/// <returns>Exchange rate</returns>
		string GetPairValue(string code1, string code2);
	}
}
