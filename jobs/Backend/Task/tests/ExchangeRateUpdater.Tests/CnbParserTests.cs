using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Tests.Resources;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
	public class CnbParserTests
	{
		[Theory]
		[InlineData("cnb_fx_rates_cs.txt", "cs-CZ")]
		[InlineData("cnb_fx_rates_en.txt", "en-US")]
		public void Parse_OK(string sourceFile, string cultureName)
		{
			var data = GetTestDataFromFile(sourceFile);
			var parser = CreateCnbParser(cultureName);

			var exchangeRates = parser.Parse(data).ToArray();

			var usdCzk = exchangeRates.Where(er => er.SourceCurrency.Code == "USD").ToArray();
			usdCzk.Should().HaveCount(1);
			usdCzk.First().TargetCurrency.Code.Should().Be("CZK");
			usdCzk.First().Value.Should().Be(22.066m);

			exchangeRates.Should().HaveCount(31);
		}

		[Theory]
		[InlineData("cnb_fx_rates_cs.txt", "cs-CZ")]
		public void Parse_SourceCurrencyAmount100_OK(string sourceFile, string cultureName)
		{
			var data = GetTestDataFromFile(sourceFile);
			var parser = CreateCnbParser(cultureName);

			var exchangeRates = parser.Parse(data).ToArray();

			var hufCzk = exchangeRates.Where(er => er.SourceCurrency.Code == "HUF").ToArray();
			hufCzk.Should().HaveCount(1);
			hufCzk.First().TargetCurrency.Code.Should().Be("CZK");
			hufCzk.First().Value.Should().Be(0.06456m);
		}

		[Fact]
		public void Parse_EmptyResult()
		{
			var data = GetTestDataFromFile("cnb_fx_rates_en_empty.txt");
			var loggerMock = CreateLoggerMock();
			loggerMock.Setup(x => x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()
			));
			var parser = CreateCnbParser(loggerMock: loggerMock);

			var exchangeRates = parser.Parse(data).ToArray();

			exchangeRates.Should().BeEmpty();
		}

		[Fact]
		public void Parse_InvalidLines()
		{
			var data = GetTestDataFromFile("cnb_fx_rates_en_invalid_lines.txt");
			var loggerMock = CreateLoggerMock();
			loggerMock.Setup(x => x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()
			));
			var parser = CreateCnbParser(loggerMock: loggerMock);

			var exchangeRates = parser.Parse(data).ToArray();

			exchangeRates.Where(er => er.SourceCurrency.Code == "AUD").Should().BeEmpty("AUD has invalid format");
			exchangeRates.Where(er => er.SourceCurrency.Code == "BRL").Should().BeEmpty("BRL has invalid format");
			exchangeRates.Where(er => er.SourceCurrency.Code == "BGN").Should().BeEmpty("BGN has invalid format");
			exchangeRates.Where(er => er.SourceCurrency.Code == "CAD").Should().BeEmpty("CAD has invalid format");
			exchangeRates.Should().HaveCount(31 - 4);
		}

		private static CnbParser CreateCnbParser(string cultureName = CnbDailyRatesOptions.DefaultCultureName, Mock<ILogger<CnbParser>>? loggerMock = null)
		{
			var options = CreateCnbDailyRatesOptions(cultureName);
			loggerMock ??= CreateLoggerMock();
			return new CnbParser(options, loggerMock.Object);
		}

		private static Mock<ILogger<CnbParser>> CreateLoggerMock() => new(MockBehavior.Strict);

		private static IOptions<CnbDailyRatesOptions> CreateCnbDailyRatesOptions(string cultureName = CnbDailyRatesOptions.DefaultCultureName)
			=> Options.Create(new CnbDailyRatesOptions()
			{
				CultureName = cultureName
			});

		private static string GetTestDataFromFile(string sourceFile) => EmbeddedResource.GetResource(sourceFile);
	}
}