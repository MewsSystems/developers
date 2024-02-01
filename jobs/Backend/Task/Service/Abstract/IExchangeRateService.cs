using ExchangeRateUpdater.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Service.Abstract
{
	public interface IExchangeRateService
	{
		public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
	}
}
