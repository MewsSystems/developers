namespace ExchangeRateUpdater.Validation {
	using System.Collections.Generic;

	public interface IValidator<in T> {
		ValidationResult Validate(T value);
		IEnumerable<ValidationResult> Validate(IEnumerable<T> collection);
	}
}
