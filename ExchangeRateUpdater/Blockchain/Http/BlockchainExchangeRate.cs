namespace Blockchain.Http {
	using Newtonsoft.Json;

	public class BlockchainExchangeRate {
		public decimal Buy { get; set; }
		public decimal Last { get; set; }
		public decimal Sell { get; set; }
		public string Symbol { get; set; }
		[JsonProperty("15m")]
		public decimal QuarterHourAgo { get; set; }
	}
}