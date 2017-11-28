using System;
using System.Collections.Generic;
using System.Text;
using ExchangeRateUpdater.Diagnostics;

namespace OpenExchangeRates.Http
{
	public class OpenExchangeRateClientOptions : IOpenExchangeRateClientOptions {
		public OpenExchangeRateClientOptions(string appId) {
			AppId = Ensure.IsNotNullOrWhiteSpace(appId, nameof(appId));
		}
		public string AppId { get; }
	}
}
