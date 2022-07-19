using ExchangeRateUpdater;

namespace Tests
{
    public class Connection
    {

        [Fact]
        public void InternetConnection()
        {
            Assert.True(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable());
        }
    }

    public class RateQueries
    {

        [Fact]
        public void EmptyQuery()
        {
            IEnumerable<Currency> currencies = new[]
            {
                new Currency("RANDOM STRING THAT WILL NOT BE FOUND AS CURRENCY")
            };

            var CNBprovider = new CNBExchangeRateProvider();
            var rates = CNBprovider.GetExchangeRates(currencies);


            var empty_list = new List<ExchangeRate>();

            Assert.Equal(empty_list, rates);
        }


        // MORE TESTS SHOULD BE ADDED HERE TO ENSURE THE FUNCTIONALITY FOR PRODUCTION ENV
    }
}
