namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
	public class Options
	{
		internal const string ConfigKey = "CzechNationalBank";

		[Required]
		public Uri Uri { get; set; }

		[Required]
		public string LineSeparator { get; set; }

		[Required]
		public string FieldSeparator { get; set; }

		[Required]
		public int LinesToSkip { get; set; }
	}
}
