using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Test.Entitites
{
    public class CurrencyTests
    {

        [Fact]
        public void Currency_New_InputNull()
        {
            Assert.Throws<CurrencyInvalidLengthException>(() => new Currency(null));
        }

        [Fact]
        public void Currency_New_InputEmpty()
        {
            Assert.Throws<CurrencyInvalidLengthException>(() => new Currency(string.Empty));
        }

        [Fact]
        public void Currency_New_LengthLessThan3()
        {
            Assert.Throws<CurrencyInvalidLengthException>(() => new Currency("AS"));
        }

        [Fact]
        public void Currency_New_LengthGreatherThan3()
        {
            Assert.Throws<CurrencyInvalidLengthException>(() => new Currency("ASSADF"));
        }

        [Fact]
        public void Currency_New_InputWithNumbers()
        {
            Assert.Throws<CurrencyMustBeAllLettersException>(() => new Currency("AB1"));
        }

        [Fact]
        public void Currency_New_InputLength3AndNoLetters()
        {
            var currency = new Currency("ABC");
            Assert.True(currency.Code == "ABC");
        }

    }

}
