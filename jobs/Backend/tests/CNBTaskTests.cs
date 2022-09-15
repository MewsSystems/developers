using CNBExchangeRateUpdater;
using System.Net.NetworkInformation;
using System.Text;

namespace Tests
{
    public class Connection
    {

        [Fact]
        public void ActiveInternetConnection()
        {
            Assert.True(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable());
        }
    }

    public class CNBWebCallTests
    {
        private List<string> _urlSources = new List<string>()
        {
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt",
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
        };


        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _exmapleData = "15 Sep 2022 #180\n\rCountry|Currency|Amount|Code|Rate\n\rAustralia|dollar|1|AUD|16.812";


        [Fact]
        public void CallingNonExistingCurrencySymbol() //calls an actual cnb web
        {
            IEnumerable<Currency> currencies = new[]
            {
                new Currency("STRING THAT IS AN INVALID CURRENCY SYMBOL")
            };

            var CNBprovider = new CNBExchangeRateProvider(_httpClient);
            var rates = CNBprovider.GetExchangeRates(currencies);


            var empty_list = new List<ExchangeRate>();

            Assert.Equal(empty_list, rates);
        }


        [Fact]
        public void ValidCallDoesntReturnNull() //calls an actual cnb web
        {
            IEnumerable<Currency> currencies = new[]
            {
                new Currency("AUD")
            };

            var CNBprovider = new CNBExchangeRateProvider(_httpClient);
            var rates = CNBprovider.GetExchangeRates(currencies);


            Assert.NotNull(rates);
        }


        [Fact]
        public void ParseRawExampleData()
        {

            var CNBprovider = new CNBExchangeRateProvider(_httpClient);
            var tuples = CNBprovider.ParseRawDataToListOfTuples(_exmapleData);
            Console.WriteLine(tuples);
            Assert.NotNull(tuples);
        }


        [Fact]
        public async void Source1StatusCodeOK()
        {
            var response = await _httpClient.GetAsync(_urlSources[0]);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async void Source2StatusCodeOK()
        {
            var response = await _httpClient.GetAsync(_urlSources[1]);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public void CNBPingServer()
        {
            var cnbIP = "193.85.3.250";
            Ping myPing = new Ping();
            PingReply reply = myPing.Send(cnbIP, 1000);
            

            Assert.NotNull(reply);
        }

        
    }
}
