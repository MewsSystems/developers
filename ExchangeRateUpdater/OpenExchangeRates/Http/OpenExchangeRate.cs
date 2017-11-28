namespace OpenExchangeRates.Http {
	using System.Collections.Generic;

	public class OpenExchangeRate {
		public string Base { get; set; }
		public string Disclaimer { get; set; }
		public string Licence { get; set; }
		public IDictionary<string,decimal> Rates { get; set; }
		public long Timestamp { get; set; }
	}
}
