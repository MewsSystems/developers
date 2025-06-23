using FluentAssertions;

namespace ExchangeRateUpdater.Tests
{
    public class CnbToolsTests
    {

        private const string CorrectCnbFileText = """
                                                  01 Sep 2024
                                                  Country|Currency|Amount|Code|Rate
                                                  USA|Dollar|1|USD|23.10
                                                  """;

        private const string CnbFileTextWithInvalidLines = """
                                                           01 Sep 2024
                                                           Country|Currency|Amount|Code|Rate
                                                           USA|Dollar|1|USD|23.10
                                                           This is a comment from CNB
                                                           Europe|Euro|1|EUR|25.10
                                                           """;

        [Fact]
        public void EmptyInputText_ShouldBeSuccessfullyParsedAndReturnEmpty()
        {
            var exchangeRates = CnbTools.ParseExchangeRates(new Currency("CZK"), "").ToList();
            exchangeRates.Should().HaveCount(0);
        }

        [Fact]
        public void CorrectInputText_ShouldBeSuccessfullyParsed()
        {
            var exchangeRates = CnbTools.ParseExchangeRates(new Currency("CZK"), CorrectCnbFileText).ToList();
            exchangeRates.Should().HaveCount(1);
            exchangeRates[0].TargetCurrency.Code.Should().Be("USD");
            exchangeRates[0].Value.Should().BeApproximately(23.10m, 0.0001m);
        }

        [Fact]
        public void CorrectInputTextWithIncorrectLines_ShouldOmitIncorrectLinesAndSucceed()
        {
            var exchangeRates = CnbTools.ParseExchangeRates(new Currency("CZK"), CnbFileTextWithInvalidLines).ToList();
            exchangeRates.Should().HaveCount(2);
            exchangeRates.Should().Contain(x => x.TargetCurrency.Code == "EUR");
            exchangeRates.Should().Contain(x => x.TargetCurrency.Code == "USD");
        }
    }
}