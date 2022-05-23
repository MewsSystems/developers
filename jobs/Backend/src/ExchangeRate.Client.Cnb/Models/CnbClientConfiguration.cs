using Framework.BaseHttpClient.Models;

namespace ExchangeRate.Client.Cnb.Models
{
	/// <summary>
	/// CNB configuration model
	/// See appsettings.json
	/// </summary>
	public class CnbClientConfiguration
	{
		/// <summary>
		/// Used for retry strategy
		/// </summary>
		public int Retry { get; set; }
		/// <summary>
		/// TTL for cache in seconds
		/// </summary>
		public int CacheTtl { get; set; }
		/// <summary>
		/// Cnb txt api configuration
		/// </summary>
		public ClientConfiguration? CnbTxtClient { get; set; }
		/// <summary>
		/// Cnb xml api configuration
		/// </summary>
		public ClientConfiguration? CnbXmlClient { get; set; }
	}
}
