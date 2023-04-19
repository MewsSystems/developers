using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services;

namespace ExchangeRateUpdater.UnitTests.Services
{
    public class CnbExchangeRatesFormatParserUnitTests
    {       
        private const string CorrectInputCnbExchangeRates = "18 Apr 2023 #75\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.364\n" +
            "Brazil|real|1|BRL|4.330\nBulgaria|lev|1|BGN|11.952\nCanada|dollar|1|CAD|15.929\n";
        private const string WrongHeaderInputCnbExchangeRates = "Header\n18 Apr 2023 #75\nCountry|Currency|Amount|Code|Rate\n" +
            "Australia|dollar|1|AUD|14.364\n" + "Brazil|real|1|BRL|4.330\nBulgaria|lev|1|BGN|11.952\nCanada|dollar|1|CAD|15.929\n";
        private const string NotNumericalExchangeRateValueInputCnbExchangeRates = "18 Apr 2023 #75\nCountry|Currency|Amount|Code|Rate\n" +
            "Australia|dollar|1|AUD|14.364\n" + "Brazil|real|1|BRL|abc\nBulgaria|lev|1|BGN|11.952\nCanada|dollar|1|CAD|15.929\n";

        private CnbExchangeRatesFormatParser _cnbExchangeRatesFormatParser;
        private IEnumerable<ExchangeRate> _expectedExchangeRates;

        [SetUp]
        public void Setup()
        {
            _cnbExchangeRatesFormatParser = new CnbExchangeRatesFormatParser();
            _expectedExchangeRates = new List<ExchangeRate> {
                new ExchangeRate(new Currency("CZK"), new Currency("AUD"), Convert.ToDecimal(14.364)),
                new ExchangeRate(new Currency("CZK"), new Currency("BRL"), Convert.ToDecimal(4.330)),
                new ExchangeRate(new Currency("CZK"), new Currency("BGN"), Convert.ToDecimal(11.952)),
                new ExchangeRate(new Currency("CZK"), new Currency("CAD"), Convert.ToDecimal(15.929))
            };
        }

        [Test]
        public void CnbExchangeRatesFormatParser_ShouldReturnProperExchangeRates_WhenCorrectInputIsProvided()
        {
            List<ExchangeRate> exchangeRates = _cnbExchangeRatesFormatParser.ParseExchangeRates(CorrectInputCnbExchangeRates).ToList();

            Assert.That(exchangeRates, Is.EqualTo(_expectedExchangeRates));
        }

        [Test]
        public void CnbExchangeRatesFormatParser_ShouldThrowFormatException_WhenBadFormattedInputIsProvided()
        {
            Assert.Throws<FormatException>(() => _cnbExchangeRatesFormatParser.ParseExchangeRates(WrongHeaderInputCnbExchangeRates));
        }        
        
        [Test]
        public void CnbExchangeRatesFormatParser_ShouldThrowFormatException_WhenExchangeRateValueIsNotNumerical()
        {
            Assert.Throws<FormatException>(() => _cnbExchangeRatesFormatParser.ParseExchangeRates(NotNumericalExchangeRateValueInputCnbExchangeRates));
        }
    }
}