namespace ExchangeRateUpdater.Financial {
	using System;
	using System.Linq;

	public class ISO4217CurrencyValidator : CurrencyValidator {
		public override bool Validate(Currency currency) {
			return !String.IsNullOrWhiteSpace(currency.Code)
				&& currency.Code.Length == 3
				&& currency.Code.All(ch => Char.IsUpper(ch));
		}
	}
}
