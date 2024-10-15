using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Provider;
using Moq;

namespace ExchangeRateUpdaterTests
{
	public class EchangeRateProviderTests
	{
		readonly Mock<IExchangeRateClient> _clientMock;
		readonly ExchangeRateProvider _provider;

		public EchangeRateProviderTests()
		{
			_clientMock = new Mock<IExchangeRateClient>();
			_provider = new ExchangeRateProvider(_clientMock.Object);
		}

		[Test]
		public void GivenSetOfCurrencies_WhenGetExchangeRatesAsyncIsCalled_ThenCorrectRatesAreReturned()
		{
			var currencies = new List<Currency>()
			{
				new Currency("USD"),
				new Currency("EUR"),
				new Currency("CZK"),
				new Currency("JPY"),
				new Currency("KES"),
				new Currency("RUB"),
				new Currency("THB"),
				new Currency("TRY"),
				new Currency("XYZ")
			};


			var rates = new List<ExchangeRateEntity>()
			{
				new ExchangeRateEntity { CurrencyCode = "USD", Rate = 23.2M },
				new ExchangeRateEntity { CurrencyCode = "EUR", Rate = 25.8M },
				new ExchangeRateEntity { CurrencyCode = "JPY", Rate = 12.2M },
				new ExchangeRateEntity { CurrencyCode = "KES", Rate = 18.2M },
				new ExchangeRateEntity { CurrencyCode = "RUB", Rate = 5.8M },
				new ExchangeRateEntity { CurrencyCode = "THB", Rate = 13.2M },
				new ExchangeRateEntity { CurrencyCode = "TRY", Rate = 18.5M }
			};

			_clientMock.Setup(c => c.GetExchangeRateEntitiesAsync(currencies)).ReturnsAsync(rates);

			var result = _provider.GetExchangeRatesAsync(currencies).Result;

			Assert.IsNotNull(result);
			Assert.That(result.Count() == 7);

			var exchangeRates = result.Select(e => e.ToString());

			Assert.That(exchangeRates.Count(e => e == "USD/CZK=23.2") == 1);
			Assert.That(exchangeRates.Count(e => e == "EUR/CZK=25.8") == 1);
			Assert.That(exchangeRates.Count(e => e == "JPY/CZK=12.2") == 1);
			Assert.That(exchangeRates.Count(e => e == "KES/CZK=18.2") == 1);
			Assert.That(exchangeRates.Count(e => e == "RUB/CZK=5.8") == 1);
			Assert.That(exchangeRates.Count(e => e == "THB/CZK=13.2") == 1);
			Assert.That(exchangeRates.Count(e => e == "TRY/CZK=18.5") == 1);
		}

		[Test]
		public void GivenEmptyListOfCurrencies_WhenGetExchangeRatesAsyncIsCalled_ThenNoRatesReturned()
		{
			var currencies = new List<Currency>();

			var rates = new List<ExchangeRateEntity>()
			{
				new ExchangeRateEntity { CurrencyCode = "USD", Rate = 23.2M },
				new ExchangeRateEntity { CurrencyCode = "EUR", Rate = 25.8M },
				new ExchangeRateEntity { CurrencyCode = "JPY", Rate = 12.2M },
				new ExchangeRateEntity { CurrencyCode = "KES", Rate = 18.2M },
				new ExchangeRateEntity { CurrencyCode = "RUB", Rate = 5.8M },
				new ExchangeRateEntity { CurrencyCode = "THB", Rate = 13.2M },
				new ExchangeRateEntity { CurrencyCode = "TRY", Rate = 18.5M }
			};

			_clientMock.Setup(c => c.GetExchangeRateEntitiesAsync(currencies)).ReturnsAsync(rates);

			var result = _provider.GetExchangeRatesAsync(currencies).Result;

			Assert.IsNotNull(result);
			Assert.That(result.Count() == 0);
		}

		[Test]
		public void GivenListOfCurrenciesWithNoExhangeRatesInSource_WhenGetExchangeRatesAsyncIsCalled_ThenNoRatesReturned()
		{
			var currencies = new List<Currency>()
			{
				new Currency("USD"),
				new Currency("EUR"),
				new Currency("CZK"),
				new Currency("JPY"),
				new Currency("KES")
			};

			var rates = new List<ExchangeRateEntity>()
			{
				new ExchangeRateEntity { CurrencyCode = "RUB", Rate = 5.8M },
				new ExchangeRateEntity { CurrencyCode = "THB", Rate = 13.2M },
				new ExchangeRateEntity { CurrencyCode = "TRY", Rate = 18.5M }
			};

			_clientMock.Setup(c => c.GetExchangeRateEntitiesAsync(currencies)).ReturnsAsync(rates);

			var result = _provider.GetExchangeRatesAsync(currencies).Result;

			Assert.IsNotNull(result);
			Assert.That(result.Count() == 0);
		}
	}
}