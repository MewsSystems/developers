using ExchangeRateUpdater.Providers.CzechNationalBank;
using Moq.Protected;
using System.Globalization;
using System.Net;
using System.Text;
using Options = ExchangeRateUpdater.Providers.CzechNationalBank.Options;

namespace ExchangeRateUpdater.Tests
{
	public class CzechNationalBankProviderTests
	{
		private const string LineSeparator = "\n";
		private const string FieldSeparator = "|";

		private const string KnownCurrency = "EUR";
		private const string UnknownCurrency = "XXX";

		private readonly Mock<HttpMessageHandler> _handler;
		private readonly Mock<IOptions<Options>> _options;

		private readonly Provider _sut;

		public CzechNationalBankProviderTests()
		{
			var options = new Options
			{
				Uri = new Uri("http://test.com"),
				LineSeparator = LineSeparator,
				FieldSeparator = FieldSeparator,
				LinesToSkip = 2
			};

			_options = new Mock<IOptions<Options>>();
			_options
				.Setup(o => o.Value)
				.Returns(options);

			var now = DateTime.Now.ToString("d MMM yyyy", CultureInfo.InvariantCulture);
			var content = new StringBuilder()
				.Append($"{now} #1")
				.Append(LineSeparator)
				.Append($"Country{FieldSeparator}Currency{FieldSeparator}Amount{FieldSeparator}Code{FieldSeparator}Rate")
				.Append(LineSeparator)
				.Append($"Country_{KnownCurrency}{FieldSeparator}Currency_{KnownCurrency}{FieldSeparator}1{FieldSeparator}{KnownCurrency}{FieldSeparator}666")
				.Append(LineSeparator)
				.ToString();

			_handler = new Mock<HttpMessageHandler>();
			_handler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

			var factory = new Mock<IHttpClientFactory>();
			factory
				.Setup(_ => _.CreateClient(Options.ConfigKey))
				.Returns(new HttpClient(_handler.Object));

			_sut = new Provider(_options.Object, factory.Object);
		}

		[Fact]
		public async Task ShouldReturnExchangeRatesForKnownCurrencies()
		{
			// Arrange
			var currencies = new List<Currency> { new Currency(KnownCurrency) };

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