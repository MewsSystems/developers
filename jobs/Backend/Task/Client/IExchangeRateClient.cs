using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
    public interface IExchangeRateClient
    {
		Task<IEnumerable<ExchangeRateEntity>> GetExchangeRateEntitiesAsync(IEnumerable<Currency> currencies);
	}
}
