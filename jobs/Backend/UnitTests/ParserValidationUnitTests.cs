using ExchangeRateUpdater.ExchangeRateGetter;
using ExchangeRateUpdater.Utils;

namespace UnitTests
{
    [TestClass]
    public class ParserValidationUnitTests
    {
        [TestMethod]
        public void CnbParserValidationTestOutOfSize()
        {
            var _exchangeGetter = new CnbExchangeRateGetter();
            var stringWithWrongLength = "Australia|dollar|1";

            var stringIsValid = _exchangeGetter.ParserValidation(stringWithWrongLength.Split(CnbConstants.EXPECTED_FILE_SEPARATOR));

            Assert.IsFalse(stringIsValid);
        }

        [TestMethod]
        public void CnbParserValidationTestAmountNotValid()
        {
            var _exchangeGetter = new CnbExchangeRateGetter();
            var stringWithWrongLength = "Australia|dollar|abc|AUD|14.764";

            var stringIsValid = _exchangeGetter.ParserValidation(stringWithWrongLength.Split(CnbConstants.EXPECTED_FILE_SEPARATOR));

            Assert.IsFalse(stringIsValid);
        }

        [TestMethod]
        public void CnbParserValidationTestRateNotValid()
        {
            var _exchangeGetter = new CnbExchangeRateGetter();
            var stringWithWrongLength = "Australia|dollar|1|AUD|rate";

            var stringIsValid = _exchangeGetter.ParserValidation(stringWithWrongLength.Split(CnbConstants.EXPECTED_FILE_SEPARATOR));

            Assert.IsFalse(stringIsValid);
        }
    }
}