using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Tests.Resources;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ExchangeRateUpdater.Tests
{
	public class CnbParserTests
	{
		// TODO Remove this field
		private readonly ITestOutputHelper output;

		public CnbParserTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Theory]
		[InlineData("cnb_fx_rates_cs_2023-06-12.txt","cs-CZ")]
		[InlineData("cnb_fx_rates_en_2023-06-12.txt","en-US")]
		public void Parse_OK(string sourceFile, string cultureName)
		{
			var data = EmbeddedResource.GetResource(sourceFile);
			var options = Options.Create(new CnbDailyRatesOptions()
			{
				CultureName = cultureName
			});
			var loggerMock = new Mock<ILogger<CnbParser>>(MockBehavior.Strict);
			var parser = new CnbParser(options, loggerMock.Object);

			var exchangeRates = parser.Parse(data).ToArray();
			
			exchangeRates.Should().HaveCount(31);
			
			var usdCzk  = exchangeRates.Where(er => er.SourceCurrency.Code == "USD").ToArray();
			usdCzk.Should().HaveCount(1);
			usdCzk.First().TargetCurrency.Code.Should().Be("CZK");
			usdCzk.First().Value.Should().Be(22.066m);
			
			var hufCzk  = exchangeRates.Where(er => er.SourceCurrency.Code == "USD").ToArray();
		}

		[Theory]
		[InlineData("cnb_fx_rates_cs_2023-06-12.txt", "cs-CZ")]
		public void Parse_SourceCurrencyAmountNot1_OK(string sourceFile, string cultureName)
		{
			var data = EmbeddedResource.GetResource(sourceFile);
			var options = Options.Create(new CnbDailyRatesOptions()
			{
				CultureName = cultureName
			});
			var loggerMock = new Mock<ILogger<CnbParser>>(MockBehavior.Strict);
			var parser = new CnbParser(options, loggerMock.Object);

			var exchangeRates = parser.Parse(data).ToArray();

			var hufCzk = exchangeRates.Where(er => er.SourceCurrency.Code == "HUF").ToArray();
			hufCzk.Should().HaveCount(1);
			hufCzk.First().TargetCurrency.Code.Should().Be("CZK");
			hufCzk.First().Value.Should().Be(0.06456m);
		}
	}
}