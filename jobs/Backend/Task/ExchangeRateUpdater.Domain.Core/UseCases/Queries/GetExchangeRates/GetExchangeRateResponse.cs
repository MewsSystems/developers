using ExchangeRateUpdater.Domain.Core.UseCases.CommonModels;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Domain.Core.UseCases.Queries.GetExchangeRates
{
	public class GetExchangeRateResponse
	{
		public IEnumerable<ExchangeRate> Rates { get; set; }
	}
}
