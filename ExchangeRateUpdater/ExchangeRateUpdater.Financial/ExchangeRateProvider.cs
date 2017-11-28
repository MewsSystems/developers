namespace ExchangeRateUpdater.Financial {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;

	public abstract class ExchangeRateProvider : IExchangeRateProvider {
		private readonly ICurrencyValidator _validator;

		public ExchangeRateProvider(ICurrencyValidator validator) {
			_validator = validator;
		}

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
		/// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies) {
			try {
				ValidateCurrencies(currencies);
				return await GetExchangeRateCoreAsync(currencies);
			} catch(Exception e) {
				throw new ApplicationException(ExceptionMessageResource.UnableToRetrieveExchangeRatesExceptionMessage, e);
			}
		}

		private void ValidateCurrencies(IEnumerable<Currency> currencies) {
			if (Check.IsNullOrEmpty(currencies)) {
				throw new ArgumentOutOfRangeException(nameof(currencies), currencies, String.Format(ExceptionMessageResource.EnumerableNullOrEmptyExceptionMessageFormat, nameof(currencies)));
			}

			var exceptions = currencies.Where(c => !_validator.Validate(c))
				.Select(c => new ArgumentException(String.Format(ExceptionMessageResource.InvalidCurrencyExceptionMessageFormat, c.Code)));

			if(!Check.IsNullOrEmpty(exceptions)) {
				throw new AggregateException(exceptions);
			}
		}

		protected abstract Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies);
		public abstract void Dispose();
	}
}
