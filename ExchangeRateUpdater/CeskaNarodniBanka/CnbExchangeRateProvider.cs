namespace CeskaNarodniBanka {
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;
	using CeskaNarodniBanka.Http;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;

	public sealed class CnbExchangeRateProvider : ExchangeRateProvider, IDisposable {
		private ICnbExchangeRateClient _client;

		public CnbExchangeRateProvider(ICnbExchangeRateClient client, ICurrencyValidator validator)
			: base(validator) {
			_client = Ensure.IsNotNull(client);
		}

		protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies) {
			var temp = await _client.GetAsync();

			var result = temp.ExchangeRates.Join(currencies, oks => oks.Code, iks => iks.Code, (t, c) => new ExchangeRate(new Currency("CZK"), new Currency(t.Code), Decimal.Parse(t.Value, CultureInfo.CreateSpecificCulture("cs-CZ"))));

			return result;
		}

		#region IDisposable implementation
		private bool isDisposed = false;

		void Dispose(bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					_client.Dispose();
					_client = null;
				}

				isDisposed = true;
			}
		}

		public override void Dispose() {
			Dispose(true);
		}
		#endregion
	}
}