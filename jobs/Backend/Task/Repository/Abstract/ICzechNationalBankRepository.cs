using ExchangeRateUpdater.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Repository.Abstract
{
	public interface ICzechNationalBankRepository
	{
		public Task<IEnumerable<ExternalCurrencyRate>> FetchCurrencyRates();
	}
}
