using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProviderException : Exception
	{
		public ExchangeRateProviderException()
		{
		}

		public ExchangeRateProviderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
