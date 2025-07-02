using ExchangeRateUpdater.Core.UseCases.CommonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Clients
{
	public interface IBankService
	{
		Task<IEnumerable<ExchangeRate>> GetExchange(string targetCurrency, DateTime? dateTime);
	}
}
