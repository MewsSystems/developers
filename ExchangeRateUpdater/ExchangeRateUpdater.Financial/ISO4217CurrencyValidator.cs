namespace ExchangeRateUpdater.Financial {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Validation;

	public class ISO4217CurrencyValidator : CurrencyValidator {
		public override ValidationResult Validate(Currency currency) {
			var result = new ValidationResult();

			if(!String.IsNullOrWhiteSpace(currency.Code)
				&& currency.Code.Length == 3
				&& currency.Code.All(ch => Char.IsUpper(ch))) {
				result.AddError(String.Format(ExceptionMessageResource.InvalidCurrencyExceptionMessageFormat, currency.Code));
			}

			return result;
		}

		public override IEnumerable<ValidationResult> Validate(IEnumerable<Currency> collection) {
			return Ensure.IsNotNullOrEmpty(collection).Select(c => Validate(c));
		}
	}
}
