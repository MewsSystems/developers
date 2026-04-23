namespace ExchangeRateUpdaterTests.Repository
{
	[TestFixture]
	public class CzechNationalBankRepositoryTests
	{
		private Mock<IConfiguration> _mockConfiguration;
		private Mock<HttpMessageHandler> _mockHttpMessageHandler;
		private CzechNationalBankRepository _repository;
		private readonly string _fakeResponse = @"
        {
            ""rates"": [
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Austrálie"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""AUD"",
                    ""rate"": 14.760
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Brazílie"",
                    ""currency"": ""real"",
                    ""amount"": 1,
                    ""currencyCode"": ""BRL"",
                    ""rate"": 4.629
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Bulharsko"",
                    ""currency"": ""lev"",
                    ""amount"": 1,
                    ""currencyCode"": ""BGN"",
                    ""rate"": 12.608
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Čína"",
                    ""currency"": ""žen-min-pi"",
                    ""amount"": 1,
                    ""currencyCode"": ""CNY"",
                    ""rate"": 3.174
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Dánsko"",
                    ""currency"": ""koruna"",
                    ""amount"": 1,
                    ""currencyCode"": ""DKK"",
                    ""rate"": 3.304
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""EMU"",
                    ""currency"": ""euro"",
                    ""amount"": 1,
                    ""currencyCode"": ""EUR"",
                    ""rate"": 24.660
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Filipíny"",
                    ""currency"": ""peso"",
                    ""amount"": 100,
                    ""currencyCode"": ""PHP"",
                    ""rate"": 40.839
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Hongkong"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""HKD"",
                    ""rate"": 2.965
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Indie"",
                    ""currency"": ""rupie"",
                    ""amount"": 100,
                    ""currencyCode"": ""INR"",
                    ""rate"": 27.931
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Indonesie"",
                    ""currency"": ""rupie"",
                    ""amount"": 1000,
                    ""currencyCode"": ""IDR"",
                    ""rate"": 1.463
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Island"",
                    ""currency"": ""koruna"",
                    ""amount"": 100,
                    ""currencyCode"": ""ISK"",
                    ""rate"": 16.741
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Izrael"",
                    ""currency"": ""nový šekel"",
                    ""amount"": 1,
                    ""currencyCode"": ""ILS"",
                    ""rate"": 5.711
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Japonsko"",
                    ""currency"": ""jen"",
                    ""amount"": 100,
                    ""currencyCode"": ""JPY"",
                    ""rate"": 15.485
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Jižní Afrika"",
                    ""currency"": ""rand"",
                    ""amount"": 1,
                    ""currencyCode"": ""ZAR"",
                    ""rate"": 1.214
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Kanada"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""CAD"",
                    ""rate"": 16.932
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Korejská republika"",
                    ""currency"": ""won"",
                    ""amount"": 100,
                    ""currencyCode"": ""KRW"",
                    ""rate"": 1.726
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Maďarsko"",
                    ""currency"": ""forint"",
                    ""amount"": 100,
                    ""currencyCode"": ""HUF"",
                    ""rate"": 6.437
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Malajsie"",
                    ""currency"": ""ringgit"",
                    ""amount"": 1,
                    ""currencyCode"": ""MYR"",
                    ""rate"": 5.167
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Mexiko"",
                    ""currency"": ""peso"",
                    ""amount"": 1,
                    ""currencyCode"": ""MXN"",
                    ""rate"": 1.065
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""MMF"",
                    ""currency"": ""SDR"",
                    ""amount"": 1,
                    ""currencyCode"": ""XDR"",
                    ""rate"": 29.005
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Norsko"",
                    ""currency"": ""koruna"",
                    ""amount"": 1,
                    ""currencyCode"": ""NOK"",
                    ""rate"": 2.340
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Nový Zéland"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""NZD"",
                    ""rate"": 14.109
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Polsko"",
                    ""currency"": ""zlotý"",
                    ""amount"": 1,
                    ""currencyCode"": ""PLN"",
                    ""rate"": 5.360
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Rumunsko"",
                    ""currency"": ""leu"",
                    ""amount"": 1,
                    ""currencyCode"": ""RON"",
                    ""rate"": 5.013
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Rusko"",
                    ""currency"": ""rublej"",
                    ""amount"": 100,
                    ""currencyCode"": ""RUB"",
                    ""rate"": 23.146
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Singapur"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""SGD"",
                    ""rate"": 16.836
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Spojené arabské emiráty"",
                    ""currency"": ""dirham"",
                    ""amount"": 1,
                    ""currencyCode"": ""AED"",
                    ""rate"": 6.271
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Spojené státy americké"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""USD"",
                    ""rate"": 22.135
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Švédsko"",
                    ""currency"": ""koruna"",
                    ""amount"": 1,
                    ""currencyCode"": ""SEK"",
                    ""rate"": 2.263
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Švýcarsko"",
                    ""currency"": ""frank"",
                    ""amount"": 1,
                    ""currencyCode"": ""CHF"",
                    ""rate"": 24.017
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Thajsko"",
                    ""currency"": ""baht"",
                    ""amount"": 100,
                    ""currencyCode"": ""THB"",
                    ""rate"": 66.904
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Turecko"",
                    ""currency"": ""lira"",
                    ""amount"": 1,
                    ""currencyCode"": ""TRY"",
                    ""rate"": 1.894
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Ukrajina"",
                    ""currency"": ""hrivna"",
                    ""amount"": 10,
                    ""currencyCode"": ""UAH"",
                    ""rate"": 7.532
                },
                {
                    ""validFor"": ""2023-10-24"",
                    ""order"": 205,
                    ""country"": ""Velká Británie"",
                    ""currency"": ""libra"",
                    ""amount"": 1,
                    ""currencyCode"": ""GBP"",
                    ""rate"": 28.339
                }
            ]
        }";

		[SetUp]
		public void Setup()
		{
			_mockConfiguration = new Mock<IConfiguration>();
			_mockConfiguration.Setup(config => config["CzechNationalBankExchangeRatesAPIUrl"]).Returns("https://api.cnb.cz/cnbapi/exrates/daily");

			_mockHttpMessageHandler = new Mock<HttpMessageHandler>();

			var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri("https://api.cnb.cz/cnbapi/exrates/daily")
			};

			_repository = new CzechNationalBankRepository(_mockConfiguration.Object, httpClient);
		}

		[Test]
		public async Task FetchCurrencyRates_ValidResponse_ReturnsExpectedRates()
		{
			// Arrange
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(_fakeResponse),
				});

			// Act
			var result = await _repository.FetchCurrencyRates();

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<IEnumerable<ExternalCurrencyRate>>(result);
		}

		[Test]
		public async Task FetchCurrencyRates_HttpRequestException_ThrowsException()
		{
			// Arrange
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ThrowsAsync(new HttpRequestException());

			// Act & Assert
			var ex = Assert.ThrowsAsync<Exception>(() => _repository.FetchCurrencyRates());
			Assert.That(ex.Message, Is.EqualTo(ErrorMessages.FailedToFetchDataFromAPI));
		}

		[Test]
		public async Task FetchCurrencyRates_WrongStatusCode_ThrowsException()
		{
			// Sample error response content
			var errorContent = JsonSerializer.Serialize(new
			{
				description = "string",
				endPoint = "string",
				errorCode = "BAD_REQUEST",
				happenedAt = "2023-10-24T20:17:26.483Z",
				messageId = "string"
			});

			// Arrange
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.BadRequest,
					ReasonPhrase = "Bad Request",
					Content = new StringContent(errorContent, Encoding.UTF8, "application/json")
				});

			// Act & Assert
			var ex = Assert.ThrowsAsync<Exception>(() => _repository.FetchCurrencyRates());

			string expectedMessage = $"API returned status code 400: Bad Request - BAD_REQUEST: string";

			Assert.That(ex.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public async Task FetchCurrencyRates_ServerError_ThrowsException()
		{
			// Arrange
			var errorContent = JsonSerializer.Serialize(new
			{
				description = "string",
				endPoint = "string",
				errorCode = "INTERNAL_SERVER_ERROR",
				happenedAt = "2023-10-24T20:17:26.483Z",
				messageId = "string"
			});

			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					ReasonPhrase = "Internal Server Error",
					Content = new StringContent(errorContent, Encoding.UTF8, "application/json")
				});

			// Act & Assert
			var ex = Assert.ThrowsAsync<Exception>(() => _repository.FetchCurrencyRates());
			string expectedMessage = $"API returned status code 500: Internal Server Error - INTERNAL_SERVER_ERROR: string";
			Assert.That(ex.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public async Task FetchCurrencyRates_NotFoundStatusCode_ThrowsException()
		{
			// Arrange
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.NotFound,
					ReasonPhrase = "Not Found",
				});

			// Act & Assert
			var ex = Assert.ThrowsAsync<Exception>(() => _repository.FetchCurrencyRates());
			string expectedMessage = $"API returned status code 404: Not Found";
			Assert.That(ex.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public async Task FetchCurrencyRates_EmptyResponse_ReturnsEmptyEnumerable()
		{
			// Arrange
			var emptyResponseContent = "{\"Rates\": []}";

			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(emptyResponseContent, Encoding.UTF8, "application/json")
				});

			// Act
			var result = await _repository.FetchCurrencyRates();

			// Assert
			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
		}
	}
}