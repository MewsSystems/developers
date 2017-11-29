namespace ExchangeRateUpdater.Financial {

	using System;
	using System.Collections.Generic;
	using ExchangeRateUpdater.Validation;

	public abstract class CurrencyValidator : ICurrencyValidator {
		protected CurrencyValidator() {
			CurrencyCodeType = CurrencyCodeType.Custom;
		}

		internal CurrencyValidator(CurrencyCodeType currencyCodeType) {
			CurrencyCodeType = currencyCodeType;
		}

		public static CurrencyValidator Create(CurrencyCodeType currencyCodeType) {
			switch (currencyCodeType) {
				case CurrencyCodeType.ISO4217:
					return new ISO4217CurrencyValidator();
				default:
					throw new InvalidOperationException();					
			}
		}

		public CurrencyCodeType CurrencyCodeType { get; }

		public abstract ValidationResult Validate(Currency value);

		public abstract IEnumerable<ValidationResult> Validate(IEnumerable<Currency> collection);
	}
}
