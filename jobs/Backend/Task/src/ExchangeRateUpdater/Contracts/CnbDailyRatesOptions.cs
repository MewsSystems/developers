using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Contracts
{
	/// <summary>
	/// Options for the CNB daily FX rates.
	/// </summary>
	public class CnbDailyRatesOptions
	{
		public const string SectionName = "CnbDaily";
		
		// Possible improvement: Use Invariant culture instead, it is defined by empty string "". That is rather ugly.
		public const string DefaultCultureName = "en-US";
		
		// Properties must be nullable because of the way how IOptions<T> works, but if any is null, the validation will fail fast.

		/// <summary>
		/// Url of the CNB daily rates service - without query string.
		/// </summary>
		[Required]
		public string? Url { get; set; }

		[Required]
		public string? CultureName { get; set; } = DefaultCultureName;
	}
}