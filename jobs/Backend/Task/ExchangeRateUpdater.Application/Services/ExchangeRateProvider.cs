using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Interfaces;

namespace ExchangeRateUpdater.Application.Services
{
	public class ExchangeRateProvider : IExchangeRateProvider
	{
		private readonly ICzechNationalBankExchangeRateClient _czechNationalBankExchangeRateClient;

		public ExchangeRateProvider(ICzechNationalBankExchangeRateClient czechNationalBankExchangeRateClient)
		{
			_czechNationalBankExchangeRateClient = czechNationalBankExchangeRateClient;
		}

		public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(DateOnly date, IEnumerable<Currency> currencies)
		{
			var exchangeRates = await _czechNationalBankExchangeRateClient.FetchExchangeRates("EN", date);

			var enumerable = currencies.ToList();
			if (enumerable.Count != 0)
			{
				return exchangeRates.Where(rate => enumerable.Any(currency => currency == rate.SourceCurrency));
			}

			return [];
		}
	}
}