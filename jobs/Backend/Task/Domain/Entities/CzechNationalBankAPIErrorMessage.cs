using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.Entities
{
	public class CzechNationalBankAPIErrorMessage
	{
		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("endPoint")]
		public string EndPoint { get; set; }

		[JsonPropertyName("errorCode")]
		public string ErrorCode { get; set; }

		[JsonPropertyName("happenedAt")]
		public string HappenedAt { get; set; }

		[JsonPropertyName("messageId")]
		public string MessageId { get; set; }
	}
}