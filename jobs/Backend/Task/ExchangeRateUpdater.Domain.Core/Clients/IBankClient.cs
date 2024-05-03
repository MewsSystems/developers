using ExchangeRateUpdater.Domain.Core.UseCases.CommonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Core.Clients
{
	public interface IBankClient
	{
		Task<IEnumerable<ExchangeRate>> GetExchange(string targetCurrency, DateTime? dateTime);
	}
}
