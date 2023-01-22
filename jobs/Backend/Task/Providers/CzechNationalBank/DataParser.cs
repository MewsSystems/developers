namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
	internal class DataParser : IDataParser
	{
		private readonly IOptions<Options> _options;

		public DataParser(IOptions<Options> options) => _options = options ?? throw new ArgumentNullException(nameof(options));

		public IEnumerable<DataRow> Parse(string data) => data
				.Split(_options.Value.LineSeparator)
				.Skip(_options.Value.LinesToSkip)
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.Select(line => DataRow.Parse(line.Split(_options.Value.FieldSeparator)));
	}
}