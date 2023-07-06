using CnbServiceClient.DTOs;
using ExchangeEntities;
using ExchangeRateTool.Interfaces;

namespace ExchangeRateTool.Services
{
	public class ExrateFilterService : IExrateFilterService
	{
        public IEnumerable<Exrate> Filter(IEnumerable<Exrate> exrates, IEnumerable<Currency> currencies)
        {
            var query = from exrate in exrates
                        join currency in currencies on exrate.CurrencyCode equals currency.Code
                        select exrate;

            return query;
        }
    }
}

