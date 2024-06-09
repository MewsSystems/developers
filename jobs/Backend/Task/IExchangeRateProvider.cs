﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public interface IExchangeRateProvider
	{
		Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
	}
}