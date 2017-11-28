﻿using ExchangeRateUpdater.Diagnostics;

namespace ExchangeRateUpdater.Financial {
	public struct Currency {
		public Currency(string code) {
			Code = Ensure.IsNotNull(code);
		}

		/// <summary>
		/// Three-letter ISO 4217 code of the currency.
		/// </summary>
		public string Code { get; private set; }
	}
}
