using models.ExchangeRateUpdater;
using Xunit;

namespace Task.UnitTests.Services
{




    public class ExchangeRateProviderServiceTests
    {

        public static IEnumerable<object[]> CurrencyModelsCorrect =>
            new List<object[]>
            {
                new object[] { new List<CurrencyModel>() {new CurrencyModel("USD"), new CurrencyModel("EUR") }},
                new object[] { new List<CurrencyModel>() {new CurrencyModel("HUF"), new CurrencyModel("CNY") }},
                new object[] { new List<CurrencyModel>() {new CurrencyModel("PLN"), new CurrencyModel("CNY") }},


            };

        public static IEnumerable<object[]> CurrencyModelsWrong =>
            new List<object[]>
            {
                new object[] { new List<CurrencyModel>() {new CurrencyModel("wrong"), new CurrencyModel("test") }},
                new object[] { new List<CurrencyModel>() {new CurrencyModel("123"), new CurrencyModel("da") }},
                new object[] { new List<CurrencyModel>() {new CurrencyModel("sd"), new CurrencyModel("1") }},


            };

        [Theory]
        [MemberData(nameof(CurrencyModelsCorrect))]
        public void CorrectInputReturnCorrectOutcome(List<CurrencyModel> currencies)
        {
            var exchangeRateProviderService = new ExchangeRateProviderService();
            var settings = new Settings()
            {
                SupportedCurrencies = new List<string>() { "EUR", "USD", "HUF", "CNY", "PLN" },
                BaseCurrency = "CZK",
                BankRatesUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml"
            };
            var result = exchangeRateProviderService.GetExchangeRates(currencies, settings);
            foreach (var res in result)
            {
                Assert.Equal("CZK", res.SourceCurrency.Code);
                Assert.NotNull(res.TargetCurrency.Code);
                Assert.NotNull(res.Value);

            }
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [MemberData(nameof(CurrencyModelsWrong))]
        public void WrongInputReturnEmptyOutcome(List<CurrencyModel> currencies)
        {
            var exchangeRateProviderService = new ExchangeRateProviderService();
            var settings = new Settings()
            {
                SupportedCurrencies = new List<string>() { "EUR", "USD", "HUF", "CNY", "PLN" },
                BaseCurrency = "CZK",
                BankRatesUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml"
            };
            var result = exchangeRateProviderService.GetExchangeRates(currencies, settings);
            Assert.Equal(0, result.Count());
        }
    }
}