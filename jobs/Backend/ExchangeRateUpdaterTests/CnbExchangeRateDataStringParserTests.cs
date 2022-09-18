using ExchangeRateUpdater;
using ExchangeRateUpdater.CzechNationalBank;
using ExchangeRateUpdater.Models;
using FluentAssertions;

namespace ExchangeRateUpdaterTests
{
	[TestClass]
	public class CnbExchangeRateDataStringParserTests
	{
		private IDataStringParser<IEnumerable<ExchangeRate>> cnbExchangeRateDataStringParser;

		[TestInitialize]
		public void TestInitialize()
		{
			cnbExchangeRateDataStringParser = new CnbExchangeRateDataStringParser();
		}

		[TestMethod]
		public void Parse_CorrectlyParsesTheCnbExchangeRateTextFormat()
		{
			var input = string.Join(
				"\n",
				"16 Sep 2022 #181",
				"Country|Currency|Amount|Code|Rate",
				"Australia|dollar|1|AUD|16.446",
				"Hungary|forint|100|HUF|6.063",
				"Indonesia|rupiah|1000|IDR|1.646"
			);

			var parsed = cnbExchangeRateDataStringParser.Parse(input);

			parsed.Should().BeEquivalentTo(new[]
			{
				new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 16.446M),
				new ExchangeRate(new Currency("HUF"), new Currency("CZK"), 0.06063M),
				new ExchangeRate(new Currency("IDR"), new Currency("CZK"), 0.001646M),
			}, options => options.WithStrictOrdering());
		}
	}
}