namespace ExchangeRateUpdater
{
	internal class App
    {
		private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		private readonly IExchangeRateProvider _provider;
		private readonly IOptions<AppOptions> _options;
		private readonly ILogger<App> _logger;

		public App(IExchangeRateProvider provider, IOptions<AppOptions> options, ILogger<App> logger)
		{
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		internal async Task<int> StartAsync(string[] _)
		{
			Console.CancelKeyPress += (sender, eventArgs) =>
			{
				_logger.LogWarning("Cancel event triggered!");
				cancellationTokenSource.Cancel();
				eventArgs.Cancel = true;
			};

			try
			{
				var currencies = _options.Value.CurrencyCodes.Select(x => new Currency(x));

				var rates = await _provider.GetExchangeRatesAsync(currencies, cancellationTokenSource.Token);

				Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
				foreach (var rate in rates)
					Console.WriteLine(rate.ToString());

				return 0;
			}
			catch (Exception e)
			{
				var message = $"Could not retrieve exchange rates: '{e.Message}'.";
				_logger.LogError(e, message);
				Console.WriteLine(message);

				return -1;
			}
		}
	}
}
