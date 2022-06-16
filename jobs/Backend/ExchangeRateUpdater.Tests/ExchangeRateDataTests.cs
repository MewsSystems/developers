namespace ExchangeRateUpdater.Tests
{
    using ExchangeRateUpdater.Code.Observability;
    using ExchangeRateUpdater.Data;
    using ExchangeRateUpdater.Domain;
    using Moq;
    using System;
    using System.Net.Http;
    using Xunit;

    public class WhenWorkingWithExchangeRateData_And_GetExchangeRateData_Is_Called
    {
        private readonly ILogger mockLogger;
        private readonly IRetryPolicy<BankDetails> mockRetryPolicy;

        public WhenWorkingWithExchangeRateData_And_GetExchangeRateData_Is_Called()
        {
            mockLogger = new Mock<ILogger>().Object;

            var mockExchangeRatePolicy = new Mock<IRetryPolicy<BankDetails>>();
            mockExchangeRatePolicy.Setup(_ => _.ExecuteWithRetry(It.IsAny<Func<BankDetails>>()))
                .Returns((Func<BankDetails> func) => func());

            mockRetryPolicy = mockExchangeRatePolicy.Object;
        }

        [Fact]
        public void When_Valid_Data_Should_Map_To_Course()
        {
            string emptyUri = $"file://{AppDomain.CurrentDomain.BaseDirectory}/Data/TestData.xml";

            var testUri = new Uri(emptyUri);

            var sut = new ExchangeRateData(testUri, mockLogger, mockRetryPolicy);

            var result = sut.GetExchangeRateData();

            Assert.IsType<BankDetails>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void When_Invalid_Data_Should_Throw_InvalidOperationException()
        {
            string emptyUri = $"file://{AppDomain.CurrentDomain.BaseDirectory}/Data/TestDataInvalid.xml";

            var testUri = new Uri(emptyUri);

            var sut = new ExchangeRateData(testUri, mockLogger, mockRetryPolicy);

            Assert.Throws<InvalidOperationException>(() => sut.GetExchangeRateData());
        }

        [Fact]
        public void When_Invalid_URI_Should_Throw_HttpRequestException()
        {
            string emptyUri = $"http://dfsgdfgfgsgsd.co.uk";

            var testUri = new Uri(emptyUri);

            var sut = new ExchangeRateData(testUri, mockLogger, mockRetryPolicy);

            Assert.Throws<HttpRequestException>(() => sut.GetExchangeRateData());
        }
    }
}