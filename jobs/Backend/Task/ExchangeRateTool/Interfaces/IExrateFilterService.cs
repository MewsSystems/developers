using CnbServiceClient.DTOs;
using ExchangeEntities;

namespace ExchangeRateTool.Interfaces
{
	public interface IExrateFilterService
	{
		IEnumerable<Exrate> Filter(IEnumerable<Exrate> exrates, IEnumerable<Currency> currencies);
	}
}

