namespace CeskaNarodniBanka {
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;
	using CeskaNarodniBanka.Http;
	using ExchangeRateUpdater.Financial;

	public sealed class CnbExchangeRateProvider : ExchangeRateProvider<ICnbExchangeRateProviderOptions> {
		private readonly string requestUriString = "denni_kurz.xml";

		public CnbExchangeRateProvider(ICnbExchangeRateProviderOptions options)
			: base(options) {
		}

		protected override string CreateRequestUriString(IEnumerable<Currency> currencies) {
			return requestUriString;
		}

		protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies) {
			var requestUrString = CreateRequestUriString(currencies);

			var temp = await Client.GetAsync<CnbExchangeRateRoot>(requestUrString);

			var result = temp.ExchangeRates.Join(currencies, oks => oks.Code, iks => iks.Code, (t, c) => new ExchangeRate(new Currency("CZK"), new Currency(t.Code), Decimal.Parse(t.Value, CultureInfo.CreateSpecificCulture("cs-CZ"))));

			return result;
		}
	}
}