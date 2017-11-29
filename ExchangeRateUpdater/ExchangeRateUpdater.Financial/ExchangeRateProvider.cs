namespace ExchangeRateUpdater.Financial {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;

	public abstract class ExchangeRateProvider<TOptions> : IExchangeRateProvider
		where TOptions : IExchangeRateProviderOptions {

		private readonly ICurrencyValidator _validator;

		public ExchangeRateProvider(TOptions options) {
			Throw.IfNull(options);

			Client = Ensure.IsNotNull(options.Client);
			_validator = Ensure.IsNotNull(options.Validator);
		}

		protected IExchangeRateClient Client { get; private set; }

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
		/// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies) {
			try {
				var validationResult = _validator.Validate(currencies);

				if (validationResult.Any(vr => !vr.IsValid)) {
					var exceptions = validationResult.SelectMany(vr => vr.Errors.Select(e => new ArgumentException(e)));

					throw new AggregateException(exceptions);
				}

				return await GetExchangeRateCoreAsync(currencies);
			} catch (Exception e) {
				throw new ApplicationException(ExceptionMessageResource.UnableToRetrieveExchangeRatesExceptionMessage, e);
			}
		}

		#region IDisposable implementation
		private bool isDisposed = false;

		void Dispose(bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					Client.Dispose();
					Client = null;
				}

				isDisposed = true;
			}
		}

		public void Dispose() {
			Dispose(true);
		}
		#endregion

		protected abstract Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies);
		protected abstract string CreateRequestUriString(IEnumerable<Currency> currencies);
	}
}
