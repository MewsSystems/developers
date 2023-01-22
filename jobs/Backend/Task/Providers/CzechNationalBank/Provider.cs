namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
	internal class Provider : IExchangeRateProvider
	{
		private const string TargetCurrency = "CZK";

		private readonly IOptions<Options> _options;
		private readonly HttpClient _client;
		private readonly IDataParser _parser;

		public Provider(IOptions<Options> options, IHttpClientFactory clientFactory, IDataParser parser)
		{
			_client = clientFactory.CreateClient(Options.ConfigKey);
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_parser = parser ?? throw new ArgumentNullException(nameof(parser));
		}

		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var mainCurrenciesTask = GetMainCurrenciesAsync(cancellationToken);
			var otherCurrenciesTask = GetOtherCurrenciesAsync(cancellationToken);

			await Task.WhenAll(new[] { mainCurrenciesTask, otherCurrenciesTask });

			var mainCurrencies = await mainCurrenciesTask;
			var otherCurrencies = await otherCurrenciesTask;

			return mainCurrencies
				.Concat(otherCurrencies)
				.Where(x => currencies.Any(c => c.Code == x.Code))
				.Select(x => new ExchangeRate(new Currency(x.Code), new Currency(TargetCurrency), x.Value))
				.ToList();
		}

		private async Task<IEnumerable<DataRow>> GetMainCurrenciesAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var data = await _client.GetStringAsync(_options.Value.MainCurrenciesUri, cancellationToken);
			return _parser.Parse(data);
		}

		private async Task<IEnumerable<DataRow>> GetOtherCurrenciesAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var data = await _client.GetStringAsync(_options.Value.OtherCurrenciesUri, cancellationToken);
			return _parser.Parse(data);
		}
	}
}
