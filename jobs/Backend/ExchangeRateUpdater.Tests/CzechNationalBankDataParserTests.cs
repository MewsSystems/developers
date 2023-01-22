using Options = ExchangeRateUpdater.Providers.CzechNationalBank.Options;

namespace ExchangeRateUpdater.Tests
{
	public class CzechNationalBankDataParserTests
	{
		private const string LineSeparator = "\n";
		private const string FieldSeparator = "|";

		private readonly Uri _mainCurrenciesUri = new("http://maincurrencies.com");
		private readonly Uri _otherCurrenciesUri = new("http://othercurrencies.com");

		private readonly IDataParser _sut;

		public CzechNationalBankDataParserTests()
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

			_sut = new DataParser(options.Object);
		}

		[Fact]
		public void ShouldParaseData()
		{
			// Arrange
			var data = $"20 Jan 2023 #15{LineSeparator}Country{FieldSeparator}Currency{FieldSeparator}Amount{FieldSeparator}Code{FieldSeparator}Rate{LineSeparator}Australia{FieldSeparator}dollar{FieldSeparator}1{FieldSeparator}AUD{FieldSeparator}15.313{LineSeparator}Brazil{FieldSeparator}real{FieldSeparator}1{FieldSeparator}BRL{FieldSeparator}4.251";

			// Act
			var result = _sut.Parse(data);

			// Assert
			Assert.Equal(2, result.Count());
		}
	}
}