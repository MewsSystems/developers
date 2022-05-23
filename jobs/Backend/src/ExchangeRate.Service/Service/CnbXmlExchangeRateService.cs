using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Client.Cnb.Models.Xml;
using ExchangeRate.Service.Abstract;
using ExchangeRate.Service.Models;

namespace ExchangeRate.Service.Service
{
	public class CnbXmlExchangeRateService : BaseExchangeRateService, IConcreteExchangeRateService
	{
		private readonly IExchangeRateClient _exchangeRateClient;

		public CnbXmlExchangeRateService(IExchangeRateClient exchangeRateClient)
		{
			_exchangeRateClient = exchangeRateClient;
		}

		public async Task<List<string>?> GetExchangeRates(string targetCurrencyCode)
		{
			XmlExchangeRate exchangeRates = await _exchangeRateClient.GetExchangeRatesXmlAsync();
			return exchangeRates.Table?.Rows?
				.Where(w => CurrencyConstants.AllowedCurrencies.Select(s => s.Code).Contains(w.Code))
				.OrderBy(o => o.Code)
				.Select(s => CreateNewExchangeRate(s.Code, s.Rate, s.Amount, targetCurrencyCode).ToString()).ToList();
		}
	}
}
