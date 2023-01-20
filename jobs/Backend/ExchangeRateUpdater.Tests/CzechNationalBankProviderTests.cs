using Options = ExchangeRateUpdater.Providers.CzechNationalBank.Options;

namespace ExchangeRateUpdater.Tests
{
	public class CzechNationalBankProviderTests
	{
		private const string LineSeparator = "\n";
		private const string FieldSeparator = "|";

		private readonly Uri _mainCurrenciesUri = new Uri("http://maincurrencies.com");
		private readonly Uri _otherCurrenciesUri = new Uri("http://othercurrencies.com");

		private const string KnownMainCurrency = "EUR";
		private const string KnownOtherCurrency = "AWG";
		private const string UnknownCurrency = "XXX";

		private const decimal CurrencyRate = 666;

		private readonly Mock<HttpMessageHandler> _handler;
		private readonly Mock<IDataParser> _parser;

		private readonly Provider _sut;

		public CzechNationalBankProviderTests()
		{
			var options = new Mock<IOptions<Options>>();
			options
				.Setup(o => o.Value)
				.Returns(new Options
				{
					MainCurrenciesUri = _mainCurrenciesUri,
					OtherCurrenciesUri = _otherCurrenciesUri,
					LineSeparator = LineSeparator,
					FieldSeparator = FieldSeparator,
					LinesToSkip = 2
				});

			var now = DateTime.Now.ToString("d MMM yyyy", CultureInfo.InvariantCulture);
			var mainCurrenciesContent = new StringBuilder()
				.Append($"{now} #1")
				.Append(LineSeparator)
				.Append($"Country{FieldSeparator}Currency{FieldSeparator}Amount{FieldSeparator}Code{FieldSeparator}Rate")
				.Append(LineSeparator)
				.Append($"Country_{KnownMainCurrency}{FieldSeparator}Currency_{KnownMainCurrency}{FieldSeparator}1{FieldSeparator}{KnownMainCurrency}{FieldSeparator}{CurrencyRate}")
				.Append(LineSeparator)
				.ToString();
			var otherCurrenciesContent = new StringBuilder()
				.Append($"{now} #1")
				.Append(LineSeparator)
				.Append($"Country{FieldSeparator}Currency{FieldSeparator}Amount{FieldSeparator}Code{FieldSeparator}Rate")
				.Append(LineSeparator)
				.Append($"Country_{KnownOtherCurrency}{FieldSeparator}Currency_{KnownOtherCurrency}{FieldSeparator}1{FieldSeparator}{KnownOtherCurrency}{FieldSeparator}{CurrencyRate}")
				.Append(LineSeparator)
				.ToString();

			_handler = new Mock<HttpMessageHandler>();
			_handler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri == _mainCurrenciesUri),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(mainCurrenciesContent) });
			_handler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri == _otherCurrenciesUri),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(otherCurrenciesContent) });

			var factory = new Mock<IHttpClientFactory>();
			factory
				.Setup(_ => _.CreateClient(Options.ConfigKey))
				.Returns(new HttpClient(_handler.Object));

			_parser = new Mock<IDataParser>();
			_parser
				.Setup(_ => _.Parse(mainCurrenciesContent))
				.Returns(new List<DataRow> { new DataRow(KnownMainCurrency, CurrencyRate) });
			_parser
				.Setup(_ => _.Parse(otherCurrenciesContent))
				.Returns(new List<DataRow> { new DataRow(KnownOtherCurrency, CurrencyRate) });

			_sut = new Provider(options.Object, factory.Object, _parser.Object);
		}

		[Fact]
		public async Task ShouldReturnExchangeRatesForKnownCurrencies()
		{
			// Arrange
			var currencies = new List<Currency> { new Currency(KnownMainCurrency), new Currency(KnownOtherCurrency) };

			// Act
			var result = await _sut.GetExchangeRatesAsync(currencies, CancellationToken.None);

			// Assert
			Assert.Equal(currencies.Count, result.Count());
		}

		[Fact]
		public async Task ShouldReturnNoDataWhenThereAreNoKnownCurrencies()
		{
			// Arrange
			var currencies = new List<Currency> { new Currency(UnknownCurrency) };

			// Act
			var result = await _sut.GetExchangeRatesAsync(currencies, CancellationToken.None);

			// Assert
			Assert.Empty(result);
		}
	}
}