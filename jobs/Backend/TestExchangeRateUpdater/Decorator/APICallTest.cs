using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Decorator;
using Moq;
using RichardSzalay.MockHttp;

namespace TestExchangeRateUpdater.Decorator
{
    public class APICallTest
    {
        [Fact]
        public async Task Load_ReturnsTrue_WhenApiReturnsSuccessAndWrapperReturnsTrue()
        {
            // Arrange
            string expectedContent = "test response";
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("*").Respond("text/plain", expectedContent);

            HttpClient httpClient = new(mockHttp);

            Mock<ILoadRates> wrapperMock = new();
            wrapperMock.Setup(w => w.Load(expectedContent)).ReturnsAsync(true);

            APICall apiCall = new(wrapperMock.Object, httpClient);

            // Act
            bool result = await apiCall.Load("01.01.2024");

            // Assert
            Assert.True(result);
            wrapperMock.Verify(w => w.Load(expectedContent), Times.Once);
        }
    }
}
