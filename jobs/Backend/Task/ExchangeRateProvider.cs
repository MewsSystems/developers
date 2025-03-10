using System.Threading.Tasks;
using ExchangeRateUpdater.Service;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		ICentralBankService _centralBankService;

		public ExchangeRateProvider(ICentralBankService centralBankService)
		{
			_centralBankService = centralBankService;
		}

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			try
			{
				var cskCurrency = new Currency("CZK");
				var exchangeRateList = new List<ExchangeRate>();
				var rates = await _centralBankService.GetExchangeRateResponse();

				currencies.ToList().ForEach(currency =>
				{
					var rate = rates.Rates.SingleOrDefault(r => r.CurrencyCode.Equals(currency.Code, System.StringComparison.InvariantCulture));
					if (rate != null)
					{
						exchangeRateList.Add(new ExchangeRate
						(
							sourceCurrency: new Currency(rate.CurrencyCode),
							targetCurrency: cskCurrency,
							value: rate.Rate
						));
					}
				});

				return exchangeRateList;

			}
			catch
			{
				throw;
			}
		}
	}
}
