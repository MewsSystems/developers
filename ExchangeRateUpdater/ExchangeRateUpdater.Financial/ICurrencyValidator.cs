namespace ExchangeRateUpdater.Financial {
	public interface ICurrencyValidator {
		bool Validate(Currency currency);
	}
}