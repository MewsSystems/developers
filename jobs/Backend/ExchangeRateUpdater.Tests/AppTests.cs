namespace ExchangeRateUpdater.Tests
{
	public class AppTests
	{
		private readonly Mock<IExchangeRateProvider> _provider;
		private readonly Mock<IOptions<AppOptions>> _options;

		private readonly App _sut;

		public AppTests()
		{
			_provider = new Mock<IExchangeRateProvider>();
			_options = new Mock<IOptions<AppOptions>>();
			_options
				.Setup(o => o.Value)
				.Returns(new AppOptions { CurrencyCodes = Enumerable.Empty<string>() });

			_sut = new App(_provider.Object, _options.Object, new Mock<ILogger<App>>().Object);
		}

		[Fact]
		public async Task ShouldExitWithCodeZeroWhenExchangeRatesAreSuccesfullyRetrieved()
		{
			// Arrange
			_provider
				.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(Enumerable.Empty<ExchangeRate>());

			// Act
			var code = await _sut.StartAsync(Array.Empty<string>());

			// Assert
			Assert.Equal(0, code);
		}

		[Fact]
		public async Task ShouldExitWithCodeMinusOneWhenProviderThrowsException()
		{
			// Arrange
			_provider
				.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
				.ThrowsAsync(new InvalidOperationException());

			// Act
			var code = await _sut.StartAsync(Array.Empty<string>());

			// Assert
			Assert.Equal(-1, code);
		}
	}
}