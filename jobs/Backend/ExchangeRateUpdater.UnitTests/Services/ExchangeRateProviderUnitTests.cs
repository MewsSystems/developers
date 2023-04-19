using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services;
using ExchangeRateUpdater.WebApi.Services.Interfaces;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Services
{
    public class ExchangeRateProviderUnitTests
    {
        private const string CorrectInputCnbExchangeRates = "18 Apr 2023 #75\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.364\n" +
            "Brazil|real|1|BRL|4.330\nBulgaria|lev|1|BGN|11.952\nCanada|dollar|1|CAD|15.929\n";

        private ExchangeRateProvider _exchangeRateProvider;

        private Mock<IExchangeRatesGetter> _exchangeRatesGetterMock;
        private Mock<IExchangeRatesParser> _exchangeRatesParserMock;

        private IEnumerable<ExchangeRate> _exchangeRatesFromParser;
        private IEnumerable<ExchangeRate> _expectedExchangeRates;

        [SetUp]
        public void Setup()
        {
            _exchangeRatesFromParser = new List<ExchangeRate> {
                new ExchangeRate(new Currency("CZK"), new Currency("AUD"), Convert.ToDecimal(14.364)),
                new ExchangeRate(new Currency("CZK"), new Currency("BRL"), Convert.ToDecimal(4.330)),
                new ExchangeRate(new Currency("CZK"), new Currency("BGN"), Convert.ToDecimal(11.952)),
                new ExchangeRate(new Currency("CZK"), new Currency("CAD"), Convert.ToDecimal(15.929))
            };

            _expectedExchangeRates = new List<ExchangeRate> {
                new ExchangeRate(new Currency("CZK"), new Currency("AUD"), Convert.ToDecimal(14.364)),
                new ExchangeRate(new Currency("CZK"), new Currency("BGN"), Convert.ToDecimal(11.952))
            };

            _exchangeRatesGetterMock = new Mock<IExchangeRatesGetter>();
            _exchangeRatesParserMock = new Mock<IExchangeRatesParser>();

            _exchangeRatesGetterMock.Setup(exchangeRatesGetter => exchangeRatesGetter.GetRawExchangeRates()).Returns(Task.FromResult(CorrectInputCnbExchangeRates));
            _exchangeRatesParserMock.Setup(exchangeRatesParser => exchangeRatesParser.ParseExchangeRates(CorrectInputCnbExchangeRates)).Returns(_exchangeRatesFromParser);

            _exchangeRateProvider = new ExchangeRateProvider(_exchangeRatesGetterMock.Object, _exchangeRatesParserMock.Object);
        }

        [Test]
        public async Task ExchangeRateProvider_ShouldReturnProperExchangeRatesValues_WhenCorrectCurrenciesAreProvided()
        {
            IEnumerable<Currency> inputCurrencies = new List<Currency> { new Currency("AUD"), new Currency("BGN"), new Currency("USD") };
            
            IEnumerable<ExchangeRate> exchangeRates = await _exchangeRateProvider.GetExchangeRates(inputCurrencies);

            Assert.That(exchangeRates, Is.EqualTo(_expectedExchangeRates));
        }
    }
}