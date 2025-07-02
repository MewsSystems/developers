using ExchangeRateUpdater.Core.UseCases.CommonModels;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Core.UseCases.Queries.GetExchangeRates
{
	public class GetExchangeRateResponse
	{
		public IEnumerable<ExchangeRate> Rates { get; set; }
	}
}
