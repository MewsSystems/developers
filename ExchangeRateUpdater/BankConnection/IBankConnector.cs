using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.BankConnection
{
	interface IBankConnector
	{
		/// <summary>
		/// Connect to API
		/// </summary>
		/// <returns>True if connected</returns>
		bool Connect();
		
		/// <summary>
		/// Close connection
		/// </summary>
		void Disconnect();
	}
}
