using System.Globalization;
using ExchangeRates.Contracts;
using ExchangeRates.Parsers;
using ExchangeRates.Providers;
using ExchangeRates.Tests.Resources;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRates.Tests.Parsers
{
	public class ParsersTestsFixture : IDisposable
	{
		private const short targetCurrencyUnitAmount = 1;
		private const decimal exchangeRateValue = 25;

		public CnbParser GetCnbParser() 
		{
			var loggerMock = GetLoggerMock<CnbParser>();
			var cultureProviderMock = GetCultureProvider<CnbCultureProvider>("cs-CZ");

			return new CnbParser(
				loggerMock.Object,
				cultureProviderMock.Object);
		}

		public string GetCnbData(string fileName) 
		{ 
			return EmbeddedResource.GetResource(fileName);
		}

		public ExchangeRate[] GetExchangeRates(string domesticCurrencyCode, params string[] foreignCurrencyCodes) 		
		{
			return foreignCurrencyCodes
				.Select(currencyCode =>
					new ExchangeRate(
						new Currency(domesticCurrencyCode), 
						new Currency(currencyCode), 
						targetCurrencyUnitAmount, 
						exchangeRateValue))
				.ToArray();
		}

		private Mock<ILogger<T>> GetLoggerMock<T>()
		{
			var logger = new Mock<ILogger<T>>(MockBehavior.Strict);
			logger.Setup(
				x => x.Log(
					It.Is<LogLevel>(l => l == LogLevel.Information || l == LogLevel.Warning),
					It.IsAny<EventId>(),
					It.IsAny<It.IsAnyType>(),
					It.IsAny<Exception>(),
					It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));

			return logger;
		}

		private Mock<TProviderType> GetCultureProvider<TProviderType>(string culture) where TProviderType : class, ICultureProvider
		{ 
			var cultureProvider = new Mock<TProviderType>(MockBehavior.Strict);
			cultureProvider
				.Setup(x => x.GetCultureInfo())
				.Returns(CultureInfo.CreateSpecificCulture(culture));

			return cultureProvider;
		}

		#region IDisposableMembers

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion IDisposableMembers	
	}
}
