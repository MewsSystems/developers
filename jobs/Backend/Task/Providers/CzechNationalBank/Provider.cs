namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
    internal class Provider : IExchangeRateProvider
	{
		private const string TargetCurrency = "CZK";

		private readonly IOptions<Options> _options;
		private readonly HttpClient _client;

		public Provider(IOptions<Options> options, IHttpClientFactory clientFactory)
		{
			_client = clientFactory.CreateClient(Options.ConfigKey);
			_options = options;
		}

		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var data = await _client.GetStringAsync(_options.Value.Uri, cancellationToken);

			var lines = data
				.Split(_options.Value.LineSeparator)
				.Skip(_options.Value.LinesToSkip)
				.Where(s => !string.IsNullOrWhiteSpace(s));

			var result = new List<ExchangeRate>();

			foreach (var line in lines)
			{
				var parts = line.Split(_options.Value.FieldSeparator);

				var code = parts[3];

				if (!currencies.Select(c => c.Code).Contains(code))
					continue;

				var amount = int.Parse(parts[2], CultureInfo.InvariantCulture);
				var rate = decimal.Parse(parts[4], CultureInfo.InvariantCulture);

				var aux = new ExchangeRate(
					new Currency(code),
					new Currency(TargetCurrency),
					rate / amount);

				result.Add(aux);
			}

			return result;
		}
	}
}
