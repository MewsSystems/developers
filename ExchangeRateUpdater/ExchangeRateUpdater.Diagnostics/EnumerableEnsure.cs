using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Diagnostics {
	public partial class Ensure {
		public static IEnumerable<T> IsNotNullOrEmpty<T>(IEnumerable<T> value) {
			if(Check.IsNullOrEmpty(value)) {
				throw new ArgumentException(ExceptionMessageResource.EnumerableNullOrEmptyExceptionMessageFormat, nameof(value));
			}

			return value;
		}
	}
}
