using ExchangeRateAPI.Controllers;
using ExchangeRateAPI.Models;
using ExchangeRateUpdater.Business;

namespace ExchangeRateAPIIntegrationTest
{
    public class ExchangeRateControllerTest
    {
        private readonly ExchangeRateController __ExchangeRateController;

        public ExchangeRateControllerTest()
        {
            __ExchangeRateController = new ExchangeRateController(new ExchangeRateProvider());
        }

        [Fact]
        public async Task ExchangeRateController_GetAsync_ShouldReturnResultsAsync()
        {
            List<ExchangeRateResponse> _Response = await __ExchangeRateController.GetAsync();

            Assert.NotNull(_Response);
            Assert.Equal(6, _Response.Count);
        }
    }
}
