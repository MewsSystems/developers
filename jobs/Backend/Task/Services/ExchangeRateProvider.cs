using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
	internal class ExchangeRateProvider : IExchangeRateProvider
	{
		internal static readonly Uri CnbExchangeRateDataUri = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");

		private readonly IWebRequestService webRequestService;
		private readonly IDataStringParser<IEnumerable<ExchangeRate>> cnbExcahngeRateDataStringParser;

		public ExchangeRateProvider(
			IWebRequestService webRequestService,
			IDataStringParser<IEnumerable<ExchangeRate>> cnbExcahngeRateDataStringParser)
		{
			this.webRequestService = webRequestService;
			this.cnbExcahngeRateDataStringParser = cnbExcahngeRateDataStringParser;
		}

		/// <summary>
		/// Returns exchange rates among the specified currencies that are defined by the source. If the source does not provide
		/// some of the specified currencies, they will not be included in the result.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
		{
			var rawExchangeRateData = await webRequestService.GetAsStringAsync(CnbExchangeRateDataUri).ConfigureAwait(false);

			var parsedExchangeRateData = cnbExcahngeRateDataStringParser.Parse(rawExchangeRateData);

			return currencies
				.Select(currency =>
					parsedExchangeRateData.FirstOrDefault(exchangeRate =>
						exchangeRate.SourceCurrency.Code == currency.Code
					)
				)
				.Where(exchangeRate => exchangeRate != null);
		}
	}
}
