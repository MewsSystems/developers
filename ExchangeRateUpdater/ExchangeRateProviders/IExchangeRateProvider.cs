using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
	public interface IExchangeRateProvider
	{
		IEnumerable<ExchangeRate> GetExchangeRates(DateTime date);
	}
}
