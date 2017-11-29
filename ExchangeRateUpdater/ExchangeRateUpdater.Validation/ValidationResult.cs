namespace ExchangeRateUpdater.Validation {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using ExchangeRateUpdater.Diagnostics;

	public class ValidationResult {
		private readonly List<string> _errors;

		public ValidationResult() {
			_errors = new List<string>();
		}

		public bool IsValid => Errors == null || Errors.Count() == 0;

		public IEnumerable<string> Errors { get; }

		public void AddError(string error) {
			_errors.Add(Ensure.IsNotNullOrWhiteSpace(error, nameof(error)));
		}
		public void AddErrorRange(string[] errors) {
			_errors.AddRange(Ensure.IsNotNullOrEmpty(errors));
		}
	}
}
