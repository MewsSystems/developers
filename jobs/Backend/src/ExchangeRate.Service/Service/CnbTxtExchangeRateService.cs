using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Client.Cnb.Models.Txt;
using ExchangeRate.Service.Abstract;
using ExchangeRate.Service.Models;

namespace ExchangeRate.Service.Service
{
	public class CnbTxtExchangeRateService : BaseExchangeRateService, IConcreteExchangeRateService
	{
		private readonly IExchangeRateClient _exchangeRateClient;

		public CnbTxtExchangeRateService(IExchangeRateClient exchangeRateClient)
		{
			_exchangeRateClient = exchangeRateClient;
		}

		public async Task<List<string>?> GetExchangeRates(string targetCurrencyCode)
		{
			IEnumerable<TxtExchangeRate> exchangeRates = await _exchangeRateClient.GetExchangeRatesTxtAsync();
			return exchangeRates
				.Where(w => CurrencyConstants.AllowedCurrencies.Select(s => s.Code).Contains(w.Code))
				.OrderBy(o => o.Code)
				.Select(s => CreateNewExchangeRate(s.Code, s.Rate, s.Amount, targetCurrencyCode).ToString()).ToList();
		}
	}
}
