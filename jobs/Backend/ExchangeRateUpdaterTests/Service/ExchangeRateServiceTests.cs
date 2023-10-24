namespace ExchangeRateUpdaterTests.Service
{
	public class ExchangeRateServiceTests
	{
		private Mock<ICzechNationalBankRepository> _mockCzechNationalBankRepository;
		private ExchangeRateService _exchangeRateService;

		[SetUp]
		public void SetUp()
		{
			_mockCzechNationalBankRepository = new Mock<ICzechNationalBankRepository>();
			_exchangeRateService = new ExchangeRateService(_mockCzechNationalBankRepository.Object);
		}

		[Test]
		public async Task GetExchangeRates_ValidCurrencies_ReturnsExpectedRates()
		{
			// Arrange
			var currencies = new List<Currency>
			{
				new Currency("USD"),
				new Currency("GBP")
			};

			var externalRates = new List<ExternalCurrencyRate>
			{
				new ExternalCurrencyRate { CurrencyCode = "USD", Rate = 20m },
				new ExternalCurrencyRate { CurrencyCode = "GBP", Rate = 25m }
			};

			_mockCzechNationalBankRepository.Setup(repo => repo.FetchCurrencyRates())
				.ReturnsAsync(externalRates);

			// Act
			var result = await _exchangeRateService.GetExchangeRates(currencies);

			// Assert
			Assert.AreEqual(2, result.Count());
			Assert.IsTrue(result.Any(r => r.SourceCurrency.Code == "USD" && r.TargetCurrency.Code == "CZK" && r.Value == 20m));
			Assert.IsTrue(result.Any(r => r.SourceCurrency.Code == "GBP" && r.TargetCurrency.Code == "CZK" && r.Value == 25m));
		}

		[Test]
		public async Task GetExchangeRates_MissingCurrency_IgnoresMissingCurrency()
		{
			// Arrange
			var currencies = new List<Currency>
			{
				new Currency("USD"),
				new Currency("ABC")
            };

			var externalRates = new List<ExternalCurrencyRate>
			{
				new ExternalCurrencyRate { CurrencyCode = "USD", Rate = 20m }
			};

			_mockCzechNationalBankRepository.Setup(repo => repo.FetchCurrencyRates())
				.ReturnsAsync(externalRates);

			// Act
			var result = await _exchangeRateService.GetExchangeRates(currencies);

			// Assert
			Assert.AreEqual(1, result.Count());
			Assert.IsTrue(result.Any(r => r.SourceCurrency.Code == "USD" && r.TargetCurrency.Code == "CZK" && r.Value == 20m));
		}

		[Test]
		public async Task GetExchangeRates_EmptyCurrenciesList_ReturnsEmptyList()
		{
			// Arrange
			var currencies = new List<Currency>();

			var externalRates = new List<ExternalCurrencyRate>
			{
				new ExternalCurrencyRate { CurrencyCode = "USD", Rate = 20m },
				new ExternalCurrencyRate { CurrencyCode = "GBP", Rate = 25m }
			};

			_mockCzechNationalBankRepository.Setup(repo => repo.FetchCurrencyRates())
				.ReturnsAsync(externalRates);

			// Act
			var result = await _exchangeRateService.GetExchangeRates(currencies);

			// Assert
			Assert.IsEmpty(result);
		}

		[Test]
		public async Task GetExchangeRates_EmptyExternalRates_ReturnsEmptyList()
		{
			// Arrange
			var currencies = new List<Currency>
			{
				new Currency("USD"),
				new Currency("GBP")
			};

			var externalRates = new List<ExternalCurrencyRate>();

			_mockCzechNationalBankRepository.Setup(repo => repo.FetchCurrencyRates())
				.ReturnsAsync(externalRates);

			// Act
			var result = await _exchangeRateService.GetExchangeRates(currencies);

			// Assert
			Assert.IsEmpty(result);
		}

		[Test]
		public async Task GetExchangeRates_WhenFetchCurrencyRatesException_ThrowsException()
		{
			// Arrange
			_mockCzechNationalBankRepository.Setup(r => r.FetchCurrencyRates())
						   .ThrowsAsync(new HttpRequestException("Generic Error"));

			// Act & Assert
			var ex = Assert.ThrowsAsync<Exception>(() => _exchangeRateService.GetExchangeRates(new List<Currency>()));
			Assert.That(ex.Message, Is.EqualTo("An error occurred while fetching currency rates."));
		}
	}
}